using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class CartRepository:RepositoryBase<Cart>,ICartRepository
    {
        public CartRepository(ECommerceDbContext context) : base(context)
        {
        }
        public override Cart Retrieve(int entityId)
        {
            var entity = this.context.Carts
                        .Include(c => c.Items)
                        .Where(c => c.ID == entityId)
                        .FirstOrDefault();
            return entity;
        }
    }
}
