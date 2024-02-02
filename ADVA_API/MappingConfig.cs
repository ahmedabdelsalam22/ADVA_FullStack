using ADVA_API.Models;
using ADVA_API.Models.DTOS;
using AutoMapper;

namespace ADVA_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<DepartmentCreateDto, Department>();
            CreateMap<Department, DepartmentDto>().ReverseMap();

            CreateMap<Employee,EmployeeDto>().ReverseMap();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<EmployeeCreateDto, Employee>();


        }
    }
}
