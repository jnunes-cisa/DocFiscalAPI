using AutoMapper;
using Repositories;
using Repositories.Helpers;
using Repositories.Query;
using Services.AutoMapper;
using System.Collections.Generic;

namespace Services
{
    public abstract class BaseServices<TEntity> where TEntity : class
    {
        public UnityOfWork UnityOfWork { get; set; }

        protected IMapper _mapper;

        protected QuerySet QuerySet;

        protected BaseServices()
        {
            _mapper = AutoMapperConfiguration.GetMapper();
        }        

        public QuerySet GetQuerySet()
        {
            return this.QuerySet;
        }

        public QuerySet ApplyQuery(Dictionary<string, string> query)
        {
            return QueryHelper.Parse(query);
        }

        public abstract TEntity Get(int id);

        public abstract List<TEntity> GetAll();

        public abstract TEntity Create(TEntity Entity);

        public abstract TEntity Update(TEntity Entity);

        public abstract TEntity Remove(int id);

        public void UpdateForType<T>(T source, ref T destination)
        {
            var fields = typeof(T).GetProperties();
            
            foreach (var itemField in fields)
            {
                if (!itemField.Name.Contains("List"))
                {
                    itemField.SetValue(destination, itemField.GetValue(source));
                }
            }
        }
    }
}