using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Supplier")]
    public class Supplier:EntityBase
    {
        public IEnumerable<ProductSupplier> ProductSuppliers { get; set; }
        public Supplier()
        {
            this.ProductSuppliers = new List<ProductSupplier>();
        }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public void AddProduct(ProductSupplier child)
        {
            ((ICollection<ProductSupplier>)this.ProductSuppliers).Add(child);
        }
        public void RemoveProduct(int productId)
        {
            var child = this.GetProduct(productId);

            ((ICollection<ProductSupplier>)this.ProductSuppliers).Remove(child);
        }
        public ProductSupplier GetProduct(int productId)
        {
            return ((ICollection<ProductSupplier>)this.ProductSuppliers)
                    .FirstOrDefault(pc => pc.SupplierID == this.ID &&
                               pc.ProductID == productId);
        }
    }
}
