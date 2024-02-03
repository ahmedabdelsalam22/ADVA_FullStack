using ADVA_FrontEnd.Models;
using ADVA_FrontEnd.Services.IServices;

namespace ADVA_FrontEnd.Services
{
    public class EmployeeService : RestSharpService<Employee>,IEmployeeService
    {
    }
}
