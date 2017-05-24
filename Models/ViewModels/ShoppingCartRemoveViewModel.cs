using System.Collections.Generic;
using Eduportal.Models.ViewModels;
 
namespace Eduportal.Models.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}