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
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetDepartments()
        {
            try
            {
                List<Department> departments = await _unitOfWork.departmentRepository.GetAll(tracked: false,
                               includes: new string[] { "Employees" });
                if (departments == null)
                {
                    return NotFound();
                }

                List<DepartmentDto> departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

                return Ok(departmentDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetDepartmentById(int? departmentId)
        {
            try
            {
                if (departmentId == 0 || departmentId == null)
                {
                    return BadRequest("department Id must't be 0 or null");
                }
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == departmentId, tracked: false,
                                   includes: new string[] { "Employees" });
                if (department == null)
                {
                    return NotFound("no department exists with this id");
                }

                DepartmentDto departmentDto = _mapper.Map<DepartmentDto>(department);

                return Ok(departmentDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost("department/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateDepartment([FromBody] DepartmentCreateDto departmentCreateDto)
        {
            try
            {
                if (departmentCreateDto == null)
                {
                    return BadRequest("invalid model");
                }

                Department department = _mapper.Map<Department>(departmentCreateDto);

                await _unitOfWork.departmentRepository.Create(department);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [HttpPut("department/update/{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateEmployee([FromBody] DepartmentDto departmentDto, int departmentId)
        {
            try
            {

                if (departmentDto == null)
                {
                    return BadRequest("invalid model");
                }
                if (departmentId != departmentDto.Id)
                {
                    return BadRequest("department Id must be tha same of department which need to Update");
                }

                Department departmentToDb = _mapper.Map<Department>(departmentDto);

                await _unitOfWork.departmentRepository.Update(departmentToDb);

                return Ok("department updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [HttpDelete("department/delete/{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteDepartment(int? departmentId)
        {
            try
            {
                if (departmentId == 0 || departmentId == null)
                {
                    return BadRequest("department Id must't be 0 or null");
                }
                Department departmentToDelete = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == departmentId, tracked: false);
                if (departmentToDelete == null)
                {
                    return NotFound();
                }
                await _unitOfWork.departmentRepository.Delete(departmentToDelete);

                return Ok("department deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
