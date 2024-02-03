using System.Text.Json.Serialization;

namespace ADVA_FrontEnd.Models.DTOS
{
    public class EmployeeUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentID { get; set; }
        public int? ManagerID { get; set; }
    }
}
