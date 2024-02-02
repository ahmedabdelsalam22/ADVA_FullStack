using ADVA_API.Models;

namespace ADVA_API.RepositoryPattern.IRepositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task Update(Employee employee);
    }
}
