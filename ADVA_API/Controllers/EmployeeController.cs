using ADVA_API.Models;
using ADVA_API.RepositoryPattern.Unit_Of_Work;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ADVA_API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _ApiResposne;
        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ApiResposne = new APIResponse();
        }

        [HttpGet("employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetEmployees()
        {
            try
            {
                List<Employee> employees = await _unitOfWork.employeeRepository.GetAll(tracked: false,
                               includes: new string[] { "Department", "Manager" });
                if (employees == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    return _ApiResposne;
                }

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                _ApiResposne.Result = employees;
                return _ApiResposne;
            }
            catch (Exception ex) 
            {
                _ApiResposne.IsSuccess = false;
                _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                _ApiResposne.ErrorMessages = new List<string>() { ex.ToString() };
                return _ApiResposne;
            }
        }
        [HttpGet("employees/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetEmployeeById(int? employeeId) 
        {
            try
            {
                if (employeeId == 0 || employeeId == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee Id must't be 0 or null" };
                    return _ApiResposne;
                }
                Employee employee = await _unitOfWork.employeeRepository.Get(filter:x=>x.Id == employeeId ,tracked: false,
                                   includes: new string[] { "Department", "Manager" });
                if (employee == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    return _ApiResposne;
                }

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                _ApiResposne.Result = employee;
                return _ApiResposne;
            }
            catch (Exception ex)
            {
                _ApiResposne.IsSuccess = false;
                _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                _ApiResposne.ErrorMessages = new List<string>() { ex.ToString() };
                return _ApiResposne;
            }
        }
        [HttpPost("employee/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee must't null" };
                    return _ApiResposne;
                }

                // we must sure that related entities is exists in database 
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Name.ToLower() == employee.Department.Name.ToLower());
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Name.ToLower() == employee.Manager.Name.ToLower());

                if (department == null && manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department or manager does't exists" };
                    return _ApiResposne;
                }

                employee.Department = department!;
                employee.Manager = manager;

                await _unitOfWork.employeeRepository.Create(employee);

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                return _ApiResposne;
            }
            catch (Exception ex) 
            {
                _ApiResposne.IsSuccess = false;
                _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                _ApiResposne.ErrorMessages = new List<string>() { ex.ToString() };
                return _ApiResposne;
            }

        }
    }
}
