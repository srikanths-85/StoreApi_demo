using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public async Task Add(T entity)
    {
        await context.Set<T>().AddAsync(entity);
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(x => x.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        T? entity = await context.Set<T>().FindAsync(id);
        return entity;
    }

    public Task Remove(T entity)
    {
        context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public Task Update(T entity)
    {
        // context.Set<T>().Attach(entity);
        // context.Entry(entity).State = EntityState.Modified;
        // return Task.CompletedTask;

        context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }
}
