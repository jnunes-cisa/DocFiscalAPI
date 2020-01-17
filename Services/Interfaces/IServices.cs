using Entities;
using Repositories.Query;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IServices<TEntity> where TEntity : EntityBase
    {

        QuerySet GetQuerySet();

        TEntity Get(int id);

        List<TEntity> GetAll();

        TEntity Create(TEntity Entity);       
         
        TEntity Update(TEntity Entity);

        TEntity Remove(int id);
    }
}
