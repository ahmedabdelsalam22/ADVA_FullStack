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
       }
    }
}
