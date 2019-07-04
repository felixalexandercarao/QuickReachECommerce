using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class ProductManufacturer
    {
        public int ManufacturerID { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
