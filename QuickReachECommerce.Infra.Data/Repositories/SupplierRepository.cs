using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class SupplierRepository:RepositoryBase<Supplier>,IRepository<Supplier>
    {
        public SupplierRepository(ECommerceDbContext context) : base(context)
        {

        }
    }
}
