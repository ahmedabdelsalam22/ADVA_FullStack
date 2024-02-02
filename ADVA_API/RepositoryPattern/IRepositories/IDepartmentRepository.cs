using ADVA_API.Models;

namespace ADVA_API.RepositoryPattern.IRepositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task Update(Department department);
    }
}
