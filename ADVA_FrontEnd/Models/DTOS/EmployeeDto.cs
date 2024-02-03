using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ADVA_FrontEnd.Models.DTOS
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee? Manager { get; set; }
    }
}
