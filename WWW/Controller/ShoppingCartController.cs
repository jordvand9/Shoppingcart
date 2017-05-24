using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Eduportal.Models;
using Eduportal.Models.ViewModels;
using Eduportal.Db;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


 
namespace Eduportal.Controllers
{
    public class ShoppingCartController : BaseController
    {
        public ShoppingCartRepository ShoppingCartRepository { get; set; }

        public ShoppingCartController(ApplicationDbContext applicationDbContext, [FromServices]ShoppingCartRepository shoppingCartRepository):base(applicationDbContext)
        {
            ShoppingCartRepository = shoppingCartRepository;
        }
        
        
        
        // GET: /ShoppingCart/
        public async Task<IActionResult> Index()
        {
            
            // Set up our ViewModel
            var viewModel = await ShoppingCartRepository.GetCart(HttpContext);
            
            // Return the view
            return View(viewModel);
        }
        public async Task<IActionResult> Empty()
        {
            await ShoppingCartRepository.EmptyCart(HttpContext);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveFromCart(Int32 id)
        {
            //Get item from cart
            var itemCount = await  ShoppingCartRepository.RemoveFromCart(id,HttpContext);
            var results = new ShoppingCartRemoveViewModel
            {

                CartTotal = await ShoppingCartRepository.GetTotal(HttpContext),
                CartCount = await ShoppingCartRepository.GetCount(HttpContext),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        public async Task<IActionResult> AddToCart(Int32 id)
        {
            await ShoppingCartRepository.AddToCart(id,HttpContext);
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> CreateOrder()
        {
            await ShoppingCartRepository.CreateOrder(HttpContext);
            return RedirectToAction("Index");
        }
        
        
    }
}