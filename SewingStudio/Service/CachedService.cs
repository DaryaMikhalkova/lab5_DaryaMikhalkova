using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using SewingStudio.Models;
//ResponseCache для соответствующих методов контроллера
namespace FuelStation.Services
{
    public class CachedService
    {
        private AtelierContext _db;
        private IMemoryCache _cache;
        private int _rowsNumber;
        private TimeSpan _delay = TimeSpan.FromSeconds(264);

        public CachedService(AtelierContext context, IMemoryCache memoryCache)
        {
            _db = context;
            _cache = memoryCache;
            _rowsNumber = 20;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            IEnumerable<Employee> emp = null;
            string name = "emp";
            if (!_cache.TryGetValue(name, out emp))
            {
                emp = _db.Employees.Include(a => a.AtelierDepartment).ToList();
                if (emp != null)
                {
                    _cache.Set(name, emp,
                    new MemoryCacheEntryOptions());
                }
            }
            return emp;
        }

        public void RefreshEmployees()
        {
            string name = "emp";
            IEnumerable<Employee> emp = _db.Employees.Include(a => a.AtelierDepartment).ToList();
            if (emp != null)
            {
                _cache.Set(name, emp);
            }
        }

        public IEnumerable<AtelierDepartment> GetAtelierDepartments()
        {
            IEnumerable<AtelierDepartment> ad = null;
            string name = "ad";
            if (!_cache.TryGetValue(name, out ad))
            {
                ad = _db.AtelierDepartments.ToList();
                if (ad != null)
                {
                    _cache.Set(name, ad);
                }
            }
            return ad;
        }
        public void RefreshAtelierDepartments()
        {
            string name = "ad";
            IEnumerable<AtelierDepartment> ad = _db.AtelierDepartments.ToList();
            if (ad != null)
            {
                _cache.Set(name, ad);
            }
        }

        public IEnumerable<Material> GetMaterials()
        {
            IEnumerable<Material> mat = null;
            string name = "mat";
            if (!_cache.TryGetValue(name, out mat))
            {
                mat = _db.Materials.ToList();
                if (mat != null)
                {
                    _cache.Set(name, mat);
                }
            }
            return mat;
        }
        public void RefreshMaterials()
        {
            string name = "mat";
            IEnumerable<Material> mat = _db.Materials.ToList();
            if (mat != null)
            {
                _cache.Set(name, mat);
            }
        }

        public IEnumerable<MaterialSupply> GetMaterialSupply()
        {
            IEnumerable<MaterialSupply> msup = null;
            string name = "msup";
            if (!_cache.TryGetValue("msup", out msup))
            {
                msup = _db.MaterialSupplies.Include(a => a.Material).ToList();
                if (msup != null)
                {
                    _cache.Set("msup", msup);
                }
            }
            return msup;
        }
        public void RefreshMaterialSupplys()
        {
            string name = "msyp";
            IEnumerable<MaterialSupply> msyp = _db.MaterialSupplies.ToList();
            if (msyp != null)
            {
                _cache.Set(name, msyp);
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            IEnumerable<Order> ord = null;
            if (!_cache.TryGetValue("ord", out ord))
            {
                ord = _db.Orders.Include(a => a.Employee).ToList();
                ord = _db.Orders.Include(a => a.Product).ToList();
                if (ord != null)
                {
                    _cache.Set("ord", ord);
                }
            }
            return ord;
        }
        public void RefreshOrders()
        {
            string name = "ord";
            IEnumerable<Order> ord = _db.Orders.ToList();
            if (ord != null)
            {
                _cache.Set(name, ord);
            }
        }
        public IEnumerable<Product> GetProducts()
        {
            IEnumerable<Product> prod = null;
            if (!_cache.TryGetValue("prod", out prod))
            {
                prod = _db.Products.Take(_rowsNumber).ToList();
                if (prod != null)
                {
                    _cache.Set("prod", prod);
                }
            }
            return prod;
        }
        public void RefreshProducts()
        {
            string name = "prod";
            IEnumerable<Product> prod = _db.Products.ToList();
            if (prod != null)
            {
                _cache.Set(name, prod);
            }
        }

    }
}

