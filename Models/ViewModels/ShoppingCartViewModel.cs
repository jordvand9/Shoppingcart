using System.Collections.Generic;
using Eduportal.Models;
 
namespace Eduportal.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public string SessionId { get; set;}
    }
}