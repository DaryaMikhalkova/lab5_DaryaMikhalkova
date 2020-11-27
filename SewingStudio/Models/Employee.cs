using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }
        public string FullName { get; set; }
        [ForeignKey("DepartmentId")]
        public AtelierDepartment AtelierDepartment { get; set; }
        public int DepartmentId { get; set; }
        public string Position { get; set; }
        public int Telephone { get; set; }
    }
}
