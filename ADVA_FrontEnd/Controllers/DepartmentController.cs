using ADVA_FrontEnd.Models;
using ADVA_FrontEnd.Models.DTOS;
using ADVA_FrontEnd.Services.IServices;
using ADVA_FrontEnd.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADVA_FrontEnd.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService,IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Department> departments = await _departmentService.GetAsync(url: $"{SD.Adva_BaseUrl}/api/departments");

            List<DepartmentDto> departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

            return View(departmentDtos);
        }

        [HttpGet]
        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentCreateDto departmentCreateDto)
        {

            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            Department department = _mapper.Map<Department>(departmentCreateDto);
            await _departmentService.PostAsync(url: $"{SD.Adva_BaseUrl}/api/department/create", data: department);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDepartment(int departmentId)
        {
            Department department = await _departmentService.GetByIdAsync(url: $"{SD.Adva_BaseUrl}/api/department/{departmentId}");

            DepartmentDto departmentDto = _mapper.Map<DepartmentDto>(department);
            return View(departmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(DepartmentDto departmentDto, int departmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Department department = _mapper.Map<Department>(departmentDto);
            await _departmentService.UpdateAsync(url: $"{SD.Adva_BaseUrl}/api/department/update/{departmentId}", data: department);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            await _departmentService.Delete(url: $"{SD.Adva_BaseUrl}/api/department/delete/{departmentId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
