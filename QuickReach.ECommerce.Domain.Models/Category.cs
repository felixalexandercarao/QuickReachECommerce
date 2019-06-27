using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class Category : EntityBase
    {
        public Category()
        {
            this.ChildCategories = new List<CategoryRollup>();
        }

        [Required]
        [MaxLength(40)]
        public  string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public bool IsActive { get; set; }
        public IEnumerable<CategoryRollup> ChildCategories { get; set; }
        public IEnumerable<CategoryRollup> ParentCategories { get; set; }

        public void AddChild(int categoryId)
        {
            if (this.ID == categoryId)
            {
                throw new ArgumentException("Child category ID must not be the same as the parent category id");
            }
            // check if category id exists

            // check if category id is not a child yet

            var child = new CategoryRollup()
            {
                ParentCategoryID = this.ID,
                ChildCategoryID = categoryId
            };
            ((ICollection<CategoryRollup>)this.ChildCategories).Add(child);
        }
    }
}
