using Skeleton.DTO;
using Skeleton.Repositories.Interfaces;

namespace Skeleton.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseDto
    {
        protected int _id { get; set; }
        protected List<T> _items { get; set; } = new List<T>();

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = _id;
            _items.Add(entity);
            _id++;
            return Task.FromResult<T>(entity);
        }

        public Task<bool> DeleteById(int id)
        {
            try
            {
                T? item = _items.FirstOrDefault(x => x.Id == id);
                if(item is null)
                {
                    return Task.FromResult<bool>(false);
                }

                _items.Remove(item);

                return Task.FromResult<bool>(true);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public Task<List<T>> GetAllAsync()
        {
            return Task.FromResult<List<T>>(_items);
        }

        public Task<T?> GetByIdAsync(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(item);
        }

        public Task UpdateAsync(T entity)
        {
            var eItem = _items.FirstOrDefault(x => x.Id == entity.Id);
            if (eItem == null) return Task.FromResult<T?>(null);

            var properties = typeof(T).GetProperties();

            foreach(var prop in properties)
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(eItem, prop.GetValue(entity));
                }
            }
            return Task.FromResult<T?>(eItem);
        }
    }
}
