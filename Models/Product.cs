using System;
using System.Collections.Generic;

namespace Eduportal.Models
{ 
     
    public enum ProductType : byte {
        Pizza = 0,
        Sandwich = 1,
        Burger  = 2,
        Desserts = 3,
        Salad = 4,
        Drink = 5
        } 
    public class Product : BaseEntity<Int32>
    {
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public Nullable<ProductType> ProductType { get; set; } 
        public List<OrderProduct> OrderProducts { get; set; }
        
        
    }
    
}
