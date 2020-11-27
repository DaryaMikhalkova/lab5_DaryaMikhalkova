using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuelStation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sewing.Models.ViewModel;
using SewingStudio.Models;

namespace SewingStudio.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AtelierContext _context;
        private readonly CachedService _cachedService;
        private readonly int _pSize = 20;
        public EmployeesController(AtelierContext context, CachedService cachedService)
        {
            _context = context;
            _cachedService = cachedService;
        }



        // GET: Employees
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            //Создание фильтра
            Func<Employee, bool> filter = a => true;
            if (Request.Cookies.ContainsKey("Filter"))
            {
                var filterValue = Request.Cookies["Filter"].Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(filterValue))
                    filter = a => a.AtelierDepartment.DepartmentName.ToLower().Contains(filterValue) || a.FullName.ToLower().Contains(filterValue) || a.Position.ToLower().Contains(filterValue)
                    || a.Telephone.ToString().Contains(filterValue);
            }
            page--;
            var cnt = _context.Employees.Count();
            //Where(filter) приминение фильтра
            return View(new DataViewModel<Employee>()
            {
                Data = _cachedService.GetEmployees().Where(filter).Skip(page * _pSize).Take(_pSize),
                PageCount = cnt / _pSize + (cnt % _pSize > 0 ? 1 : 0),
                CurrentPage = page
            });
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _cachedService.GetEmployees()
                .FirstOrDefault(m => m.IdEmployee == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.AtelierDepartments, "IdDepartment", "DepartmentName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmployee,FullName,DepartmentId,Position,Telephone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                _cachedService.RefreshEmployees();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.AtelierDepartments, "IdDepartment", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _cachedService.GetEmployees().FirstOrDefault(m => m.IdEmployee == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_cachedService.GetAtelierDepartments(), "IdDepartment", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmployee,FullName,DepartmentId,Position,Telephone")] Employee employee)
        {
            if (id != employee.IdEmployee)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    _cachedService.RefreshEmployees();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.IdEmployee))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.AtelierDepartments, "IdDepartment", "DepartmentName", employee.DepartmentId);

            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.AtelierDepartment)
                .FirstOrDefaultAsync(m => m.IdEmployee == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            _cachedService.RefreshEmployees();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.IdEmployee == id);
        }
    }
}
