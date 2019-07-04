using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.Domain.Models
{
    public class Cart:EntityBase
    {
        public int CustomerID { get; set; }
        public IEnumerable<CartItem> Items { get; set; }
        public Cart(int customerID)
        {
            CustomerID = customerID;
            Items = new List<CartItem>();
        }
        public void RemoveCartItem(string productId, string id)
        {
            var child = this.GetCartItem(productId,id);

            ((ICollection<CartItem>)this.Items).Remove(child);
        }
        public CartItem GetCartItem(string productId,string id)
        {
            return ((ICollection<CartItem>)this.Items)
                    .FirstOrDefault(pc => pc.Id== id &&
                               pc.ProductId == productId);
        }

        public void AddCartItem(CartItem cartItem)
        {
            ((ICollection<CartItem>)this.Items).Add(cartItem);
        }
    }
}