using Microsoft.EntityFrameworkCore;
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext Context;
        internal DbSet<T> dbSet;
        
        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
            
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
           // return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            dbSet.Add(entity);
            //await dbSet.SaveChangesAsync();
            return entity;
        }

        public int Count()
        {
            return dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await dbSet.CountAsync();
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return dbSet.SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return dbSet.Where(match).ToList();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await dbSet.SingleOrDefaultAsync(match);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = dbSet.Where(predicate);
            return query;
        }

        public async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            return await dbSet
                .Where(predicate).ToListAsync();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }// to check

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> 
            filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // include properties will be comma separated
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public async Task<ICollection<T>> GetAllAsyn()
        {
            return await dbSet.ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public T GetFirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // include properties will be comma separated
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
            Remove(entityToRemove);
        }
    }
}
