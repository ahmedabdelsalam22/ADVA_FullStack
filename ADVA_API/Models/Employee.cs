using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADVA_API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public int? ManagerID { get; set; }
        public virtual Employee Manager { get; set; }
    }
}
