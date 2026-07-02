using System.Linq.Expressions;

namespace SalarySlipManagementApi.Repositories
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string includeProperties = "");
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> expression,
            string includeProperties = ""
        );
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
