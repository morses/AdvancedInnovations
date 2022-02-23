using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using DiscordStats.DAL.Abstract;

namespace DiscordStats.DAL.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        // The context is private to enforce full separation, preventing a subclass from accessing
        // entities other than this one.  If you need other entities, write a separate "service" or "provider" class
        // that has access to all repositories needed
        private readonly DbContext _context;
        // This one is OK to use in a subclass because it only represents the entity for this type of repository
        // and it can be mocked
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext ctx)
        {
            _context = ctx;
            _dbSet = _context.Set<TEntity>();   // must do it this way because we don't have a "navigation property" to use
        }

        public TEntity FindById(int id)
        {
            TEntity entity = _dbSet.Find(id);
            return entity;  // null if not found
        }

        public bool Exists(int id)
        {
            return FindById(id) != null;
        }

        public IQueryable<TEntity> GetAll()
        {
            // note, no includes here and we're returning it as an IQueryable, NOT a DbSet, on purpose
            // so the caller cannot do other things (which should go here or in a subclass)
            return _dbSet;
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            // Apply includes one by one
            IQueryable<TEntity> dbs = _dbSet;
            foreach (var item in includes)
            {
                dbs = dbs.Include(item);
            }
            return dbs;
        }

        public TEntity AddOrUpdate(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            _context.Update(entity);
            _context.SaveChangesAsync();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("Entity to delete was not found or was null");
            }
            else
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
            return;
        }

        public void DeleteById(int id)
        {
            Delete(FindById(id));
            return;
        }

    }
}
