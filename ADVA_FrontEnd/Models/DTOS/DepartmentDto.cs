using System.ComponentModel.DataAnnotations;

namespace ADVA_FrontEnd.Models.DTOS
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
