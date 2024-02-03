using ADVA_FrontEnd.Models;
using ADVA_FrontEnd.Models.DTOS;
using ADVA_FrontEnd.Services.IServices;
using ADVA_FrontEnd.Utilities;
using AutoMapper;
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
    }
}
