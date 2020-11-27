using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    public class Material
    {
        [Key]
        public int IdMaterial { get; set; }
        public string MaterialName { get; set; }
        public string MaterialType { get; set; }
        public int QuantityMaterialInStock { get; set; }
    }
}
