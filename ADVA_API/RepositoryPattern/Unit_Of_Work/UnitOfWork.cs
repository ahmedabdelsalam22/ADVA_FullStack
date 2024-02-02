using ADVA_API.DataAccess;
using ADVA_API.RepositoryPattern.IRepositories;
using ADVA_API.RepositoryPattern.Repositories;

namespace ADVA_API.RepositoryPattern.Unit_Of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            employeeRepository = new EmployeeRepository(context);
            departmentRepository = new DepartmentRepository(context);
        }
        public IEmployeeRepository employeeRepository { get; private set; }
        public IDepartmentRepository departmentRepository { get; private set; }
    }
}
