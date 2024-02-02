using ADVA_API.DataAccess;
using ADVA_API.Models;
using ADVA_API.RepositoryPattern.IRepositories;

namespace ADVA_API.RepositoryPattern.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Update(Department department)
        {
            _context.Update(department);
            await _context.SaveChangesAsync();
        }
    }
}
