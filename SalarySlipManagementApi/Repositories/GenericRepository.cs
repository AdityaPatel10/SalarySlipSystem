using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalarySlipManagementApi.Data;

namespace SalarySlipManagementApi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        internal DbSet<T> _dbSet;

        public GenericRepository(SalarySlipDbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            foreach (
                var includeProperty in includeProperties.Split(
                    new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> expression,
            string includeProperties = ""
        )
        {
            IQueryable<T> query = _dbSet;

            foreach (
                var includeProperty in includeProperties.Split(
                    new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(expression).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            var isActiveProperty = entity.GetType().GetProperty("IsActive");
            if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
            {
                isActiveProperty.SetValue(entity, true);
            }

            var DOJProperty = entity.GetType().GetProperty("DateOfJoining");
            if (DOJProperty != null && DOJProperty.PropertyType == typeof(DateTime))
            {
                DOJProperty.SetValue(entity, DateTime.UtcNow);
            }

            var generatedOnProperty = entity.GetType().GetProperty("GeneratedOn");
            if (generatedOnProperty != null && generatedOnProperty.PropertyType == typeof(DateTime))
            {
                generatedOnProperty.SetValue(entity, DateTime.UtcNow);
            }

            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            var updatedAtProperty = entity.GetType().GetProperty("UpdatedAt");
            if (updatedAtProperty != null && updatedAtProperty.PropertyType == typeof(DateTime?))
            {
                updatedAtProperty.SetValue(entity, DateTime.UtcNow);
            }

            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            var isActiveProperty = entity.GetType().GetProperty("IsActive");

            if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
            {
                isActiveProperty.SetValue(entity, false);

                var deletedAtProperty = entity.GetType().GetProperty("DeletedAt");
                if (
                    deletedAtProperty != null
                    && deletedAtProperty.PropertyType == typeof(DateTime?)
                )
                {
                    deletedAtProperty.SetValue(entity, DateTime.UtcNow);
                }
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
