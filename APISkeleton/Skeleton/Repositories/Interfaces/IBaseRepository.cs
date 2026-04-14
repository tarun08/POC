using Skeleton.DTO;

namespace Skeleton.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseDto
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> DeleteById(int id);
    }
}
