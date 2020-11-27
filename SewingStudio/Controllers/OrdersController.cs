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
    public class OrdersController : Controller
    {
        private readonly AtelierContext _context;
        private readonly CachedService _cachedService;
        private readonly int _pSize = 20;

        public OrdersController(AtelierContext context, CachedService cachedService)
        {
            _context = context;
            _cachedService = cachedService;
        }

        // GET: Orders
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            page--;
            var cnt = _context.Orders.Count();
            return View(new DataViewModel<Order>()
            {
                Data = _cachedService.GetOrders().Skip(page * _pSize).Take(_pSize),
                PageCount = cnt / _pSize + (cnt % _pSize > 0 ? 1 : 0),
                CurrentPage = page
            });
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _cachedService.GetOrders()
                .FirstOrDefault(m => m.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "IdEmployee", "FullName");
            ViewData["ProductNameID"] = new SelectList(_context.Products, "IdProduct", "ProductName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrder,CustomerName,ProductNameID,EmployeeID,NumberOfProducts,Price,OrderDate,CheckSaleDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "IdEmployee", "FullName", order.EmployeeID);
            ViewData["ProductNameID"] = new SelectList(_context.Products, "IdProduct", "ProductName", order.ProductNameID);
            _cachedService.RefreshOrders();
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _cachedService.GetOrders().FirstOrDefault(m => m.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "IdEmployee", "FullName", order.EmployeeID);
            ViewData["ProductNameID"] = new SelectList(_context.Products, "IdProduct", "ProductName", order.ProductNameID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrder,CustomerName,ProductNameID,EmployeeID,NumberOfProducts,Price,OrderDate,CheckSaleDate")] Order order)
        {
            if (id != order.IdOrder)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    _cachedService.RefreshOrders();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.IdOrder))
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
            ViewData["EmployeeID"] = new SelectList(_context.Products, "IdEmployee", "FullName", order.EmployeeID);
            ViewData["ProductNameID"] = new SelectList(_context.Products, "IdProduct", "ProductName", order.ProductNameID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            _cachedService.RefreshOrders();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.IdOrder == id);
        }
    }
}
