using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public string CustomerName { get; set; }
        public int ProductNameID { get; set; }
        [ForeignKey("ProductNameID")]
        public Product Product { get; set; }
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
        public int NumberOfProducts { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CheckSaleDate { get; set; }
    }
}
