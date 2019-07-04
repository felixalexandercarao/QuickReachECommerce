using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ECommerceDbContext context) : base(context)
        {

        }
        public override IEnumerable<Customer> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context
                    .Set<Customer>()
                    .AsNoTracking()
                    .Where(c => c.FirstName.Contains(search) ||
                                c.LastName.Contains(search) ||
                                c.ZipCode.Contains(search) ||
                                c.City.Contains(search) ||
                                c.State.Contains(search) ||
                                c.Country.Contains(search) ||
                                c.CardHolderName.Contains(search))
                    .Skip(skip)
                    .Take(count)
                    .ToList();

            return result;
        }

    }
}
