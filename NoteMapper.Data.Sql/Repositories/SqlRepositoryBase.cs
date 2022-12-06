using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NoteMapper.Core;

namespace NoteMapper.Data.Sql
{
    public abstract class SqlRepositoryBase<T> where T : class
    {
        private readonly NoteMapperContext _context;

        protected SqlRepositoryBase(NoteMapperContext context)
        {
            _context = context;
        }

        public Task<ServiceResult> CreateAsync(T entity)
        {
            Set().Add(entity);
            return SaveChangesAsync();
        }

        public Task<ServiceResult> UpdateAsync(T entity)
        {
            return SaveChangesAsync();
        }

        protected Task<ServiceResult> DeleteRangeAsync(IReadOnlyCollection<T> entities)
        {
            if (entities.Count == 0)
            {
                return Task.FromResult(ServiceResult.Successful());
            }
            
            Set().RemoveRange(entities);
            return SaveChangesAsync();
        }

        protected async Task<ServiceResult> DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        {
            IReadOnlyCollection<T> entities = await Set()
                .Where(predicate)
                .ToArrayAsync();

            return await DeleteRangeAsync(entities);
        }

        protected Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return Set()
                .FirstOrDefaultAsync(predicate);
        }

        protected async Task<IReadOnlyCollection<T>> Select(Expression<Func<T, bool>> predicate)
        {
            return await Set()
                .Where(predicate)
                .ToArrayAsync();
        }                

        private async Task<ServiceResult> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync(true);
                return ServiceResult.Successful();
            }
            catch
            {
                return ServiceResult.Failure("");
            }
        }

        private DbSet<T> Set()
        {
            return _context.Set<T>();
        }
    }
}