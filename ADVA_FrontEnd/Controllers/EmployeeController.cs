using ADVA_FrontEnd.Models;
using ADVA_FrontEnd.Models.DTOS;
using ADVA_FrontEnd.Services.IServices;
using ADVA_FrontEnd.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADVA_FrontEnd.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService,IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Employee> employees = await _employeeService.GetAsync(url: $"{SD.Adva_BaseUrl}/api/employees");

            List<EmployeeDto> employeesDtos = _mapper.Map<List<EmployeeDto>>(employees);

            return View(employeesDtos);
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeCreateDto employeeCreateDto)
        {

            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            Employee employee = _mapper.Map<Employee>(employeeCreateDto);
            await _employeeService.PostAsync(url: $"{SD.Adva_BaseUrl}/api/employee/create", data: employee);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int employeeId)
        {
            Employee employee = await _employeeService.GetByIdAsync(url: $"{SD.Adva_BaseUrl}/api/employee/{employeeId}");

            EmployeeUpdateDto employeeUpdateDt = _mapper.Map<EmployeeUpdateDto>(employee);
            return View(employeeUpdateDt);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(EmployeeUpdateDto employeeUpdateDto, int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Employee employee = _mapper.Map<Employee>(employeeUpdateDto);
            await _employeeService.UpdateAsync(url: $"{SD.Adva_BaseUrl}/api/employee/update/{employeeId}", data: employee);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            await _employeeService.Delete(url: $"{SD.Adva_BaseUrl}/api/employee/delete/{employeeId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
