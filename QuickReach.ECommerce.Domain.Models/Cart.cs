using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.Domain.Models
{
    public class Cart:EntityBase
    {
        public int CustomerID { get; set; }
        public List<CartItem> Items { get; set; }
        public Cart()
        {
        }
    }
}
