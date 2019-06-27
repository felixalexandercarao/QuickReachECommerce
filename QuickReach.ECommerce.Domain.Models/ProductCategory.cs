using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class ProductCategory
    {
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

    }
}

