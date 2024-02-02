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
        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                List<Employee> employees = await _unitOfWork.employeeRepository.GetAll(tracked: false,
                               includes: new string[] { "Department", "Manager" });
                if (employees == null)
                {
                    return NotFound();
                }

                List<EmployeeDto> employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);

                return Ok(employeeDtos);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetEmployeeById(int? employeeId) 
        {
            try
            {
                if (employeeId == 0 || employeeId == null)
                {
                    return BadRequest("employee Id must't be 0 or null");
                }
                Employee employee = await _unitOfWork.employeeRepository.Get(filter:x=>x.Id == employeeId ,tracked: false,
                                   includes: new string[] { "Department", "Manager" });
                if (employee == null)
                {
                    return NotFound();
                }

                EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPost("employee/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            try
            {
                if (employeeCreateDto == null)
                {
                    return BadRequest("employee must't null");
                }

                // we must sure that related entities is exists in database 
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == employeeCreateDto.DepartmentID);
                
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeCreateDto.ManagerID);

                if (department == null && manager == null)
                {
                    return NotFound();
                }

                employeeCreateDto.Department = department!;
                employeeCreateDto.Manager = manager;

                Employee employeeToDb = _mapper.Map<Employee>(employeeCreateDto);

                await _unitOfWork.employeeRepository.Create(employeeToDb);

                return Created();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message.ToString());
            }

        }
        [HttpPut("employee/update/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeUpdateDto employeeDto , int employeeId)
        {
            try
            {
                
                if (employeeDto == null)
                {
                    return BadRequest("employee must't be null");
                }
                if (employeeId != employeeDto.Id)
                {
                    return BadRequest("employee Id must be tha same of employee which need to Update");
                }
                Employee employeeFromDb = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeId, tracked: false);
                if (employeeFromDb == null)
                {
                    return NotFound("no employee found with this id");
                }

                // we must sure that related entities is exists in database 
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == employeeDto.DepartmentID);

                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeDto.ManagerID);

                if (department == null && manager == null)
                {
                    return NotFound("department or manager does't exists");
                }

                employeeDto.Department = department!;
                employeeDto.Manager = manager;


                Employee employeeToDb = _mapper.Map<Employee>(employeeDto);

                await _unitOfWork.employeeRepository.Update(employeeToDb);

                return Ok("employee updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }
        [HttpDelete("employee/delete/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteEmployee(int? employeeId) 
        {
            try
            {
                if (employeeId == 0 || employeeId == null)
                {
                    return BadRequest("employee Id must't be 0 or null");
                }
                Employee employeeToDelete = await _unitOfWork.employeeRepository.Get(filter: x => x.Id == employeeId, tracked: false);
                if (employeeToDelete == null)
                {
                    return NotFound("no employee found with this id");
                }
                await _unitOfWork.employeeRepository.Delete(employeeToDelete);
                return Ok("employee deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
