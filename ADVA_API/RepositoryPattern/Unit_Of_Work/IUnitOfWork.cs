using ADVA_API.RepositoryPattern.IRepositories;

namespace ADVA_API.RepositoryPattern.Unit_Of_Work
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository employeeRepository { get; }
        public IDepartmentRepository departmentRepository { get; }
    }
}
