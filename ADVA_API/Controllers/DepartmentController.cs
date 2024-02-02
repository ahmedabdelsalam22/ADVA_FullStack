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
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _ApiResposne;
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ApiResposne = new APIResponse();
        }

        [HttpGet("departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetDepartments()
        {
            try
            {
                List<Department> departments = await _unitOfWork.departmentRepository.GetAll(tracked: false,
                               includes: new string[] { "Manager", "Employees" });
                if (departments == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    return _ApiResposne;
                }

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                _ApiResposne.Result = departments;
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

        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetDepartmentById(int? departmentId)
        {
            try
            {
                if (departmentId == 0 || departmentId == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    _ApiResposne.ErrorMessages = new List<string>() { "department Id must't be 0 or null" };
                    return _ApiResposne;
                }
                Department department = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == departmentId, tracked: false,
                                   includes: new string[] { "Manager", "Employees" });
                if (department == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    return _ApiResposne;
                }

                _ApiResposne.IsSuccess = true;
                _ApiResposne.StatusCode = HttpStatusCode.OK;
                _ApiResposne.Result = department;
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

        [HttpPost("department/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateDepartment([FromBody] Department departmentToCreate)
        {
            try
            {
                if (departmentToCreate == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department must't null" };
                    return _ApiResposne;
                }

                // we must sure that related entities is exists in database
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Name.ToLower() == departmentToCreate.Manager.Name.ToLower());

                if (manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "manager does't exists" };
                    return _ApiResposne;
                }

                departmentToCreate.Manager = manager;

                await _unitOfWork.departmentRepository.Create(departmentToCreate);

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

        [HttpPost("department/update/{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateEmployee([FromBody] Department departmentToUpdate, int departmentId)
        {
            try
            {

                if (departmentToUpdate == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department must't be null" };
                    return _ApiResposne;
                }
                if (departmentId != departmentToUpdate.Id)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "department Id must be tha same of department which need to Update" };
                    return _ApiResposne;
                }
                Department departmentFromDb = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == departmentId, tracked: false);
                if (departmentFromDb == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    _ApiResposne.ErrorMessages = new List<string>() { "department does't exists" };
                    return _ApiResposne;
                }

                // we must sure that related entities is exists in database 
                Employee manager = await _unitOfWork.employeeRepository.Get(filter: x => x.Name.ToLower() == departmentToUpdate.Manager.Name.ToLower());

                if (manager == null)
                {
                    _ApiResposne.IsSuccess = false;
                    _ApiResposne.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResposne.ErrorMessages = new List<string>() { "manager does't exists" };
                    return _ApiResposne;
                }

                departmentToUpdate.Manager = manager;

                await _unitOfWork.departmentRepository.Update(departmentToUpdate);

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

        [HttpDelete("department/delete/{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteDepartment(int? departmentId)
        {
            try
            {
                if (departmentId == 0 || departmentId == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    _ApiResposne.ErrorMessages = new List<string>() { "department Id must't be 0 or null" };
                    return _ApiResposne;
                }
                Department departmentToDelete = await _unitOfWork.departmentRepository.Get(filter: x => x.Id == departmentId, tracked: false);
                if (departmentToDelete == null)
                {
                    _ApiResposne.IsSuccess = true;
                    _ApiResposne.StatusCode = HttpStatusCode.NotFound;
                    return _ApiResposne;
                }
                await _unitOfWork.departmentRepository.Delete(departmentToDelete);
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
