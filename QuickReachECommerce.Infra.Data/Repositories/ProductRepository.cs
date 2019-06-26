using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Domain.NewExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ECommerceDbContext context) : base(context)
        {

        }

        public IEnumerable<Product> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Products
                .Where(c => c.Name.Contains(search) || c.Description.Contains(search))
                .Skip(skip)
                .Take(count)
                .ToList();

            return result;
        }
        public override Product Create(Product newEntity)
        {
            var checklist = this.context.Categories
                .Where(c => c.ID == newEntity.CategoryID);
            if (checklist.Count() == 0)
            {
                throw new CategoryDoesntExist();
            }
            this.context.Products
                        .Add(newEntity);
            this.context.SaveChanges();
            return newEntity;
        }
    }
}
