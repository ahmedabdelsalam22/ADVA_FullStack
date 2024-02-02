using System.ComponentModel.DataAnnotations;

namespace ADVA_API.Models.DTOS
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public int? ManagerID { get; set; }
        public virtual Employee Manager { get; set; }
    }
}
