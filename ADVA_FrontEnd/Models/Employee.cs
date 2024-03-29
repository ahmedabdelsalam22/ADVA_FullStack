﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADVA_FrontEnd.Models
{
    public class Employee
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
