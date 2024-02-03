using System.ComponentModel.DataAnnotations;

namespace ADVA_FrontEnd.Models.DTOS
{
    public class DepartmentCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
