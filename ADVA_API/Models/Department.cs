using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ADVA_API.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
