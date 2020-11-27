using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SewingStudio.Models
{
    public class AtelierContext : DbContext
    {
        public DbSet<AtelierDepartment> AtelierDepartments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialSupply> MaterialSupplies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public AtelierContext(DbContextOptions<AtelierContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
