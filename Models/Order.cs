using System;
using System.Collections.Generic;

namespace Eduportal.Models
{ 
    public enum Status : byte {
        Order_Placed = 0,
        Order_Processed = 1,
        Order_Shipped  = 2,
        Order_Delivered = 3
        } 
    public class Order : BaseEntity<Int32>
    {
    public decimal Total     { get; set; }
         
    public Nullable<Status> Status  { get; set; } 
    public List<OrderProduct> OrderProducts { get; set; }
    
    public Int32 CustomerId { get; set; }
    public Customer Customer { get; set; } 

    public Int32 DriverId { get; set; }
    public Driver Driver { get; set; }  

    public Int16 RestaurantId { get; set;}
    public Restaurant Restaurant { get; set; } 
    public List<OrderDetail> OrderDetails { get; set; }
      
    }
}
