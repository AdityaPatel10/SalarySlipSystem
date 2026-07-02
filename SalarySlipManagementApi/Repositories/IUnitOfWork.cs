namespace SalarySlipManagementApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Entities.Employee> Employees { get; }
        IGenericRepository<Entities.Role> Roles { get; }
        IGenericRepository<Entities.Department> Departments { get; }
        IGenericRepository<Entities.SalaryStructure> SalaryStructures { get; }
        IGenericRepository<Entities.MonthlySalarySlip> MonthlySalarySlips { get; }
        Task<int> CompleteAsync();
    }
}
