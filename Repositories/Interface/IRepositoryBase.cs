using Repositories.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repositories.RepositoryBase
{

    public interface IRepositoryBase<TEntity> where TEntity : class
    {

        TEntity Get(Expression<Func<TEntity, bool>> expression = null, params string[] include);

        TEntity Get(Func<TEntity, bool> predicate = null, params Expression<Func<TEntity, object>>[] includeExpressions);

        TEntity Get(QuerySet querySet);

        List<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null, params string[] include);

        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeExpressions);

        List<TEntity> GetAll(string include, Expression<Func<TEntity, bool>> expression = null);

        List<TEntity> GetAll(ref QuerySet querySet);

        TEntity Add(TEntity entity);

        TEntity AddAll(TEntity entity);

        TEntity Update(TEntity entity, string propField = "Id");

        TEntity UpdateAll(TEntity entity);

        TEntity Remove(TEntity entity, string propField = "Id");

        TEntity RemoveAll(TEntity entity);

        TEntity Inative(TEntity entity);

        void Attach(TEntity entity);

        void AttachAll(TEntity entity);

        void Detached(TEntity entity);

    }

}