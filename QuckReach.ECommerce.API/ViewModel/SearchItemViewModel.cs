using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuckReach.ECommerce.API.ViewModel
{
    public class SearchItemViewModel
    {
        public int ProductID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
