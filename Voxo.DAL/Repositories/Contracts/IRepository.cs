
namespace University.DAL.Repositories.Contracts
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IList<T>> GetAllAsync();
        Task<T> GetAsync(int? id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task AddAsync(T entity);
      
    }
}
