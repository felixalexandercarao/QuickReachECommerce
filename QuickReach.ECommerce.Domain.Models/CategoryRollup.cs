using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class CategoryRollup
    {
        public int ParentCategoryID { get; set; }
        public Category ParentCategory { get; set; }

        public int ChildCategoryID { get; set; }
        public Category ChildCategory { get; set; }
    }
}

