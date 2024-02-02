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
        [HttpGet("employee/{employeeId}")]
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
        public async Task<ActionResult<APIResponse>> CreateEmployee([FromBody] Employee employeeToCreate)
        {
            try
            {
                if (employeeToCreate == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee must't null" };
                    return _ApiResposne;
                }

                // we must sure that related entities is exists in database 
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Name.ToLower() == employeeToCreate.Department.Name.ToLower());
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Name.ToLower() == employeeToCreate.Manager.Name.ToLower());

                if (department == null && manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department or manager does't exists" };
                    return _ApiResposne;
                }

                employeeToCreate.Department = department!;
                employeeToCreate.Manager = manager;

                await _unitOfWork.employeeRepository.Create(employeeToCreate);

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

        [HttpPost("employee/update/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateEmployee([FromBody] Employee employeeToUpdate , int employeeId)
        {
            try
            {
                
                if (employeeToUpdate == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee must't be null" };
                    return _ApiResposne;
                }
                if (employeeId != employeeToUpdate.Id)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee Id must be tha same of employee which need to Update" };
                    return _ApiResposne;
                }
                Employee employeeFromDb = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeId, tracked: false);
                if (employeeFromDb == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee does't exists" };
                    return _ApiResposne;
                }

                // we must sure that related entities is exists in database 
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Name.ToLower() == employeeToUpdate.Department.Name.ToLower());
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Name.ToLower() == employeeToUpdate.Manager.Name.ToLower());

                if (department == null && manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department or manager does't exists" };
                    return _ApiResposne;
                }

                employeeToUpdate.Department = department!;
                employeeToUpdate.Manager = manager;

                await _unitOfWork.employeeRepository.Update(employeeToUpdate);

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

        [HttpDelete("employee/delete/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteEmployee(int? employeeId) 
        {
            try
            {
                if (employeeId == 0 || employeeId == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee Id must't be 0 or null" };
                    return _ApiResposne;
                }
                Employee employeeToDelete = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeId, tracked: false);
                if (employeeToDelete == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    return _ApiResposne;
                }
                await _unitOfWork.employeeRepository.Delete(employeeToDelete);
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
