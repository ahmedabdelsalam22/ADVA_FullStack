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
        [ForeignKey(name: "DepartmentID")]
        public virtual Department Department { get; set; }
        public int? ManagerID { get; set; }
        [ForeignKey(name: "ManagerID")]
        public virtual Employee Manager { get; set; }
    }
}
