using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories.Entities;
using Repositories.Extensions;
using Repositories.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repositories.RepositoryBase
{

    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {

        protected DbContext context;
        protected DbSet<TEntity> dbSet;

        public RepositoryBase(DbContext _context)
        {
            this.context = _context;
            this.dbSet = context.Set<TEntity>();
        }

        #region GET

        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression = null, params string[] include)
        {
            IQueryable<TEntity> query = dbSet.AsNoTracking();

            if (include != null && include.Count() > 0)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(expression);
        }

        public virtual TEntity Get(Func<TEntity, bool> predicate = null, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return includeExpressions
              .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(dbSet.AsNoTracking(), (current, expression) => current.Include(expression))
              .FirstOrDefault(predicate);
        }

        public virtual TEntity Get(QuerySet querySet)
        {

            IQueryable<TEntity> entities = dbSet.AsNoTracking();

            if (querySet.Filters.Count > 0)
                foreach (var filter in querySet.Filters)
                    entities = entities.Filter(filter);

            return entities.Sort(querySet.SortParameters).FirstOrDefault();

        }

        #endregion

        #region GET ALL

        public virtual List<TEntity> GetAll(string include, Expression<Func<TEntity, bool>> expression = null)
        {
            return GetAll(expression, include.Split(","));
        }

        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null, params string[] include)
        {

            IQueryable<TEntity> query = dbSet.AsNoTracking();

            if (include != null && include.Count() > 0)
            {
                query = query.Include(include); 
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.ToList();

        }

        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = dbSet.AsNoTracking();

            if (includeExpressions.Count() > 0)
            {
                query = includeExpressions.Aggregate(query, (current, expressionInclude) => current.Include(expressionInclude));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }

        public virtual List<TEntity> GetAll(ref QuerySet querySet)
        {

            if (querySet == null)
            {
                querySet = new QuerySet { ReturnResult = new ReturnResult { TotalRegisters = dbSet.Count(), TotalPages = 0, CurrentPage = 1, PageSize = 0 } };
                querySet.ReturnResult.TotalRegistersPage = querySet.ReturnResult.TotalRegisters;

                return dbSet.ToList();
            }

            querySet.ReturnResult = new ReturnResult();

            IQueryable<TEntity> entities = dbSet;

            entities = entities.Include(querySet.IncludedRelationships.ToArray());

            if (querySet.Filters != null)
                querySet.Filters.ForEach(r => entities = entities.Filter(r));

            querySet.ReturnResult.TotalRegisters = entities.Count();

            entities = querySet.PageQuery == null
              ? entities.Sort(querySet.SortParameters).Select(querySet.Fields)
              : entities.Sort(querySet.SortParameters).Select(querySet.Fields).Page(querySet.PageQuery.PageSize, querySet.PageQuery.PageOffset);

            if (querySet.PageQuery != null)
            {
                querySet.ReturnResult.PageSize = querySet.PageQuery.PageSize;
                querySet.ReturnResult.CurrentPage = querySet.PageQuery.PageOffset;

                querySet.ReturnResult.TotalPages = (querySet.ReturnResult.PageSize > 0) ? (int)Math.Ceiling((decimal)querySet.ReturnResult.TotalRegisters / querySet.ReturnResult.PageSize) : 1;
            }

            return entities.ToList();

        }

        #endregion

        #region Persistencia

        private EntityEntry GetTracker(TEntity entity, string propName)
        {
            var trackerEntrie = context.ChangeTracker.Entries().FirstOrDefault(r => {

                Type t = r.Entity.GetType();
                PropertyInfo p = t.GetProperty(propName);
                object trackerValue = p.GetValue(r.Entity, null);

                t = entity.GetType();
                p = t.GetProperty(propName);
                object entityValue = p.GetValue(entity, null);

                return r.Entity is TEntity && trackerValue.ToString() == entityValue.ToString();

            });

            return trackerEntrie;
        }

        public virtual TEntity Add(TEntity entity)
        {

            if (entity is EntityBase)
            {
                (entity as EntityBase).DataCriacao = DateTime.Now;
                (entity as EntityBase).DataAlteracao = DateTime.Now;
            }

            context.Entry(entity).State = EntityState.Added;

            return entity;

        }

        public virtual TEntity AddAll(TEntity entity)
        {
            if (entity is EntityBase)
            {
                (entity as EntityBase).DataCriacao = DateTime.Now;
                (entity as EntityBase).DataAlteracao = DateTime.Now;
            }

            return dbSet.Add(entity).Entity;
        }

        public virtual TEntity UpdateAll(TEntity entity)
        {
            if (entity is EntityBase)
            {
                (entity as EntityBase).DataAlteracao = DateTime.Now;
            }

            return dbSet.Update(entity).Entity;
        }

        public virtual TEntity Update(TEntity entity, string propField = "Id")
        {

            if (entity is EntityBase)
            {
                (entity as EntityBase).DataAlteracao = DateTime.Now;
            }

            var trackerEntrie = GetTracker(entity, propField); 

            if (trackerEntrie != null)
            {                
                context.Entry(trackerEntrie.Entity).State = EntityState.Modified;
                context.Entry(trackerEntrie.Entity).CurrentValues.SetValues(entity);
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            return entity;

        }

        public virtual TEntity RemoveAll(TEntity entity)
        {
            return dbSet.Remove(entity).Entity;
        }

        public virtual TEntity Remove(TEntity entity, string propField = "Id")
        {
            var trackerEntrie = GetTracker(entity, propField);

            if (trackerEntrie != null)
            {
                trackerEntrie.State = EntityState.Deleted;
            }
            else
            {
                context.Entry(entity).State = EntityState.Deleted;
            }

            return entity;

        }

        public virtual TEntity Inative(TEntity entity)
        {
            if (entity is EntityBase)
            {                
                (entity as EntityBase).DataAlteracao = DateTime.Now;

                var entry = dbSet.Attach(entity);               
                entry.Property("DataAlteracao").IsModified = true;

                return dbSet.Update(entity).Entity;
            }

            return entity;
        }

        #endregion

        public virtual void Attach(TEntity entity)
        {   
            context.Entry(entity).State = EntityState.Unchanged;
        }

        public virtual void AttachAll(TEntity entity)
        {
            dbSet.Attach(entity);
        }

        public virtual void Detached(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Detached;
        }

    }

}