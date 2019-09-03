using System;
using System.Collections.Generic;
using System.Text;
using QuickReach.ECommerce.Domain;
using System.Linq;
using QuickReach.ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly ECommerceDbContext context;
        public RepositoryBase(ECommerceDbContext context)
        {
            this.context = context;
        }
        public virtual TEntity Create(TEntity newEntity)
        {
            this.context.Set<TEntity>()
                        .Add(newEntity);
            this.context.SaveChanges();
            return newEntity;
        }

        public virtual void Delete(int entityID)
        {
            var entityToRemove = Retrieve(entityID);
            this.context.Remove<TEntity>(entityToRemove);
            this.context.SaveChanges();
        }

        public virtual TEntity Retrieve(int entityID)
        {
            var entity = this.context
                             .Set<TEntity>()
                             .AsNoTracking()
                             .FirstOrDefault(c=>c.ID==entityID);
            return entity;
        }


        public IEnumerable<TEntity> Retrieve(int skip = 0, int count = 10)
        {
            var result = this.context.Set<TEntity>()
                             .AsNoTracking()
                             .Skip(skip)
                             .Take(count)
                             .ToList();
            return result;
        }

        public virtual IEnumerable<TEntity> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(int entityID, TEntity entity)
        {
            this.context.Update<TEntity>(entity);
            this.context.SaveChanges();
            return entity;
        }
    }
}
