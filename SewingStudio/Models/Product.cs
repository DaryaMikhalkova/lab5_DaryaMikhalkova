using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public int MaterialNameId { get; set; }
        [ForeignKey("MaterialNameId")]
        public Material Material { get; set; }
        public string NumberOfMaterialsPerItem { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }
    }
}
