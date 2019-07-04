using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class Order:EntityBase
    {
        public int CustomerID { get; set; }
        public List<OrderItem> Items { get; set; }
        public int CartID { get; set; }
        public Order()
        {
        }
    }
}
