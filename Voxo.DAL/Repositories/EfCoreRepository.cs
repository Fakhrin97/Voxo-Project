
namespace University.DAL.Repositories
{
    public class EfCoreRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly AppDbContext _dbContext;

        public EfCoreRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetAsync(int? id)
        {
            if (id == null) throw new Exception();

            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

}
