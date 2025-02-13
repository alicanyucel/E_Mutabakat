﻿using É_Mutabakat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace É_Mutabakat.Core.DataAccess.EntityFramework
{
    public class EfEntityFrameworkBase<TEntity, TContext> : IEntityRepository<TEntity>
          where TEntity : class, IEntity, new()
        where TContext : DbContext,new()
    {
        public void Add(TEntity entity)
        {
            using (var context=new TContext())
            {
                var addedentity = context.Entry(entity);
                addedentity.State = EntityState.Added;
                context.SaveChanges();  
            }                   
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedentity = context.Entry(entity);
                deletedentity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);

            }
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter =null)
        {
            using (var context = new TContext())
            {
                return filter == null ? context.Set<TEntity>().ToList() :
                     context.Set<TEntity>().Where(filter).ToList();

            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedentity = context.Entry(entity);
                updatedentity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
