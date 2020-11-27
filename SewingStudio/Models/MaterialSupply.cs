using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    [Table("MaterialSupply")]
    public class MaterialSupply
    { 
        [Key]
        public int MaterialSuplyId { get; set; }
        public string Provider { get; set; }
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
        public int PriceOfMaterials { get; set; }
        public int AmountOfMaterial { get; set; }
        public DateTime DeliveryDate { get; set; }

    }
}
