using BilgeShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Repositories
{
    // TEntity yerine hangi entity classını gönderirsem repository onun için çalışacak. Böylelikle her bir entity için ayrı ayrı repository açmama gerek yok. ---> GENERIC REPOSITORY PATTERN
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);
        void Update(TEntity entity);
        TEntity GetById(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);

    }
}
