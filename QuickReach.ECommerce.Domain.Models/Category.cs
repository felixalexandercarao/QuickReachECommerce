using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Category")]
    public class Category : EntityBase
    { 
        [Required]
        [MaxLength(40)]
        public  string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public IEnumerable<Product> Products { get; set; }

        public bool IsActive { get; set; }
    }
}
