using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ADVA_API.Models.DTOS
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentID { get; set; }
        [JsonIgnore]
        public virtual Department Department { get; set; }
        public int? ManagerID { get; set; }
        [JsonIgnore]
        public virtual Employee Manager { get; set; }
    }
}
