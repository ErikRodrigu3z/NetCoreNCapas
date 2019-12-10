using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Common
{

    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();  
        IEnumerable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate); 
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Edit(TEntity entity);
    }

    public abstract class EntityBase
    {
        public int Id { get; protected set; }
    }

}
