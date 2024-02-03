using ADVA_FrontEnd.Models;
using ADVA_FrontEnd.Models.DTOS;
using AutoMapper;

namespace ADVA_FrontEnd.Utilities
{
    public class MappingConfig : Profile
    {
       public MappingConfig() 
       {
            CreateMap<Employee , EmployeeDto>();
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<DepartmentCreateDto, Department>();
        }
    }
}
