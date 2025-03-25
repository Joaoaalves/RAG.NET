using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Interfaces;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Métodos sem verificação de userId (para quando não for aplicável)
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Sobrecargas que fazem a verificação do UserId para entidades que implementam IUserOwned

        public async Task<T?> GetByIdAsync(Guid id, string? userId)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is IUserOwned owned)
            {
                return owned.UserId == userId ? entity : null;
            }
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? userId)
        {
            if (typeof(IUserOwned).IsAssignableFrom(typeof(T)))
            {
                return await _dbSet.Where(e => EF.Property<string>(e, "UserId") == userId).ToListAsync();
            }
            else
            {
                return await GetAllAsync();
            }
        }

        public async Task UpdateAsync(T entity, string? userId)
        {
            if (entity is IUserOwned owned)
            {
                if (owned.UserId != userId)
                    throw new UnauthorizedAccessException("User not allowed to update this entity.");
            }
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity, string? userId)
        {
            if (entity is IUserOwned owned)
            {
                if (owned.UserId != userId)
                    throw new UnauthorizedAccessException("User not allowed to delete this entity.");
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
