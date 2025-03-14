using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task Add (T entity);
    Task Update (T entity);
    Task Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
}
