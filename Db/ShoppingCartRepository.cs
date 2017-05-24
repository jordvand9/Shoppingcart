using System;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Eduportal.Models;
using Eduportal.Models.ViewModels;
using Eduportal.Models.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eduportal.Models.Security;

namespace Eduportal.Db
{
    public class ShoppingCartRepository : BaseRepository
    {        
        public ShoppingCartRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {
        }

        public async Task<ShoppingCartViewModel> GetCart(HttpContext context) 
        {
            
            var model = new ShoppingCartViewModel()
            {
                CartItems = await ApplicationDbContext.CartItems.Where(c => c.CartId == GetCartId(context)).Include(c => c.Product).ToListAsync(),
                CartTotal = await ApplicationDbContext.CartItems.Include(c => c.Product).Where(c => c.CartId == GetCartId(context)).Select(c => c.Product.Price * c.Count).SumAsync(), 
                SessionId = GetCartId(context)
            };

            return model;
        }
        public async Task AddToCart(int id, HttpContext context)
        {
            //Get the right cart & Product
            var cartItem = await ApplicationDbContext.CartItems.SingleOrDefaultAsync(c => c.CartId == GetCartId(context) && c.ProductId == id); 
            //If cart is empty add a new item to the cart
            if(cartItem == null)
            {
                cartItem = new CartItem()
                {
                    ProductId = id,
                    CartId = GetCartId(context),
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                //Add the item to the cart
                ApplicationDbContext.CartItems.Add(cartItem);
            }
            else
            {
                //Item already exists so add one to the counter
                cartItem.Count++;
            }
            ApplicationDbContext.SaveChanges();
        }
        public async Task EmptyCart(HttpContext context)
        {
            //Get all items where Cartid is the current one
            var cartItems = await ApplicationDbContext.CartItems.Where(c => c.CartId == GetCartId(context)).ToListAsync();
            //Remove the items from the cart
            ApplicationDbContext.CartItems.RemoveRange(cartItems);
            ApplicationDbContext.SaveChanges();
            
        }
        public async Task<int> RemoveFromCart(int cartItemId, HttpContext context)
        {
            //Get the current cart and albums
            var cartItems = await ApplicationDbContext.CartItems.SingleOrDefaultAsync(c => c.CartId == GetCartId(context) && c.CartItemId == cartItemId);
            //Set the item counter to 0
            int itemCount = 0;
            //Check if cart is empty
            if (cartItems != null)
                {
                    if (cartItems.Count > 1)
                    {
                        cartItems.Count--;
                        itemCount = cartItems.Count;
                    }
                    else
                    {
                        ApplicationDbContext.CartItems.Remove(cartItems);
                        
                        
                    }
                    //Save changes
                    ApplicationDbContext.SaveChanges();
                    
                }
                
                return itemCount;
        }
        public async Task<decimal> GetTotal(HttpContext context)
        {
            var cartItems = await ApplicationDbContext.CartItems.Include(c => c.Product).Where(c => c.CartId ==GetCartId(context)).Select(c => c.Product.Price * c.Count).SumAsync();
            return cartItems;
                    
        }
        // We're using HttpContextBase to allow access to sessions.
        private static string GetCartId(HttpContext context)
        {
            var cartId = context.Session.GetString("Session");

            if (cartId == null)
            {
                //A GUID to hold the cartId. 
                cartId = Guid.NewGuid().ToString();

                // Send cart Id as a cookie to the client.
                context.Session.SetString("Session", cartId);
            }

            return cartId;
        }
        public Task<List<CartItem>> GetCartItems(HttpContext context)
        {
            return ApplicationDbContext.CartItems.Where(c => c.CartId == GetCartId(context)).Include(c => c.Product).ToListAsync();
        }
        public Task<List<string>> GetCartProductTitles(HttpContext context)
        {
            return ApplicationDbContext.CartItems.Where(c => c.CartId == GetCartId(context)).Select(c => c.Product.Name).OrderBy(n => n).ToListAsync();
        }
        public Task<int> GetCount(HttpContext context)
            {
                // Get the count of each item in the cart and sum them up
                return ApplicationDbContext.CartItems.Where(c => c.CartId == GetCartId(context)).Select(c => c.Count).SumAsync();
            }
        public async Task CreateOrder(HttpContext context)
        {
            decimal orderTotal = 0;
            
                var cartItems = await GetCartItems(context);
                //Get the current user so we can assign the CustomerId later
                var person =  await ApplicationDbContext.Customer.FirstOrDefaultAsync(c => c.Email == context.User.Identity.Name);
                var personId = person.Id;
                var orderr = new Order{
                        
                        Name = "Testr",
                        Description = personId.ToString(),
                        CustomerId = personId,
                        DriverId = 153,
                        RestaurantId = 2,
                    };
                    ApplicationDbContext.Order.Add(orderr);
                    ApplicationDbContext.SaveChanges();
                // Iterate over the items in the cart, adding the order details for each
                foreach (var item in cartItems)
                {
                    //var album = _db.Products.Find(item.ProductId);
                    var album = await ApplicationDbContext.Products.SingleAsync(a => a.Id == item.ProductId);
                    
                    // Create a new orderDetail
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = orderr.Id,
                        UnitPrice = album.Price,
                        Quantity = item.Count,
                        
                    };

                    // Set the order total of the shopping cart
                    orderTotal += (item.Count * album.Price);

                    ApplicationDbContext.OrderDetails.Add(orderDetail);
                }

                // Set the order's total to the orderTotal count
                orderr.Total = orderTotal;

                // Empty the shopping cart after converting to an order
                await EmptyCart(context);
                  
        }

        
    }
}