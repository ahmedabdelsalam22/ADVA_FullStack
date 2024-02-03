using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ADVA_FrontEnd.Models.DTOS
{
    public class EmployeeCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public int DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
        public int? ManagerID { get; set; }
        public virtual Employee? Manager { get; set; }
    }
}
