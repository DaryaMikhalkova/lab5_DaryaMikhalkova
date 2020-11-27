using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    public class AtelierDepartment
    {
        [Key]
        public int IdDepartment { get; set; }
        public string DepartmentName { get; set; }
        public int AmountOfWorkers { get; set; }
        public string DescriptionOfTheTypeOfWork { get; set; }
    }
}
