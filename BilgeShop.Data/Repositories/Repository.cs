using BilgeShop.Data.Context;
using BilgeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BilgeShop.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {

        private readonly BilgeShopContext _db;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(BilgeShopContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();

            // _dbSet yerine _db.Users / _db.Products / _db.Categories gelecek.
        }

        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            _db.SaveChanges();
            
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            _db.SaveChanges();
        }
    }
}


//Find: Id parametresi ile nesne varsa hemen yakalar.En az performans harcayan yöntemdir.
//First:Eşleşen ilk veriyi buluyor.Eğer arananı bulamazsa hata fırlatır.
//FirstOrDefault:İlk veriyi bulur bulamazsa null döner
//Single:ilk veriyi bulur başka veriyi bulur.Başka veriyi bulursa hata fırlatır
//SingleOrDefault:ilk veriyi bulur, bulamazsa null döner, birden fazla bulursa hata fırlatır.