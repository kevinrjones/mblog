using System.Data.Entity;
using System.Linq;

namespace Repository
{
    public abstract class BaseEfRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbSet<T> _dbSet;
        private readonly DbContext _dataDbContext;

        protected BaseEfRepository(DbContext dataDbContext)
        {
            _dataDbContext = dataDbContext;
            _dbSet = dataDbContext.Set<T>();
        }

        public IQueryable<T> Entities
        {
            get { return _dbSet; }
        }

        public T New()
        {
            return _dbSet.Create();
        }

        public void Add(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        protected internal void Save()
        {
            _dataDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dataDbContext.Dispose();
        }
    }
}