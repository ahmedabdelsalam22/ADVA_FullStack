using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ADVA_FrontEnd.Models.DTOS
{
    public class EmployeeCreateDto
    {
        public string Name { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public int DepartmentID { get; set; }
        [JsonIgnore]
        public virtual Department? Department { get; set; }
        public int? ManagerID { get; set; }
        [JsonIgnore]
        public virtual Employee? Manager { get; set; }
    }
}
