using SalarySlipManagementApi.Data;
using SalarySlipManagementApi.Entities;

namespace SalarySlipManagementApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SalarySlipDbContext _context;
        public IGenericRepository<Entities.Employee> Employees { get; private set; }

        public IGenericRepository<Entities.Role> Roles { get; private set; }
        public IGenericRepository<Entities.Department> Departments { get; private set; }
        public IGenericRepository<Entities.SalaryStructure> SalaryStructures { get; private set; }
        public IGenericRepository<Entities.MonthlySalarySlip> MonthlySalarySlips
        {
            get;
            private set;
        }

        public UnitOfWork(SalarySlipDbContext context)
        {
            _context = context;
            Employees = new GenericRepository<Entities.Employee>(_context);
            Roles = new GenericRepository<Entities.Role>(_context);
            Departments = new GenericRepository<Entities.Department>(_context);
            SalaryStructures = new GenericRepository<Entities.SalaryStructure>(_context);
            MonthlySalarySlips = new GenericRepository<Entities.MonthlySalarySlip>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
