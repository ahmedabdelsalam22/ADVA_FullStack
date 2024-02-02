using System.ComponentModel.DataAnnotations;

namespace ADVA_API.Models.DTOS
{
    public class DepartmentCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
