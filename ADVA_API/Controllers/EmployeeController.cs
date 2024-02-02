using ADVA_API.Models;
using ADVA_API.Models.DTOS;
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

                List<EmployeeDto> employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                _ApiResposne.Result = employeeDtos;
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

                EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                _ApiResposne.Result = employeeDto;
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
        public async Task<ActionResult<APIResponse>> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            try
            {
                if (employeeCreateDto == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee must't null" };
                    return _ApiResposne;
                }

                // we must sure that related entities is exists in database 
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == employeeCreateDto.DepartmentID);
                
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeCreateDto.ManagerID);

                if (department == null && manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department or manager does't exists" };
                    return _ApiResposne;
                }

                employeeCreateDto.Department = department!;
                employeeCreateDto.Manager = manager;

                Employee employeeToDb = _mapper.Map<Employee>(employeeCreateDto);

                await _unitOfWork.employeeRepository.Create(employeeToDb);

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
        [HttpPut("employee/update/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateEmployee([FromBody] EmployeeDto employeeDto , int employeeId)
        {
            try
            {
                
                if (employeeDto == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "employee must't be null" };
                    return _ApiResposne;
                }
                if (employeeId != employeeDto.Id)
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
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == employeeDto.DepartmentID);

                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeDto.ManagerID);

                if (department == null && manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department or manager does't exists" };
                    return _ApiResposne;
                }

                employeeDto.Department = department!;
                employeeDto.Manager = manager;


                Employee employeeToDb = _mapper.Map<Employee>(employeeDto);

                await _unitOfWork.employeeRepository.Update(employeeToDb);

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
