using ADVA_API.DataAccess;
using ADVA_API.Models;
using ADVA_API.RepositoryPattern.IRepositories;

namespace ADVA_API.RepositoryPattern.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
