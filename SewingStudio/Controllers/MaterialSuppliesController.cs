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
    public class MaterialSuppliesController : Controller
    {
        private readonly AtelierContext _context;
        private readonly CachedService _cachedService;
        private readonly int _pSize = 20;

        public MaterialSuppliesController(AtelierContext context, CachedService cachedService)
        {
            _context = context;
            _cachedService = cachedService;
        }

        // GET: MaterialSupplies
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
           
            page--;
            var cnt = _context.MaterialSupplies.Count();
            return View(new DataViewModel<MaterialSupply>()
            {
                Data = _cachedService.GetMaterialSupply().Skip(page * _pSize).Take(_pSize),
                PageCount = cnt / _pSize + (cnt % _pSize > 0 ? 1 : 0),
                CurrentPage = page
            });
        }

        // GET: MaterialSupplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialSupply = _cachedService.GetMaterialSupply()
                .FirstOrDefault(m => m.MaterialSuplyId == id);
            if (materialSupply == null)
            {
                return NotFound();
            }

            return View(materialSupply);
        }

        // GET: MaterialSupplies/Create
        public IActionResult Create()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "IdMaterial", "MaterialName");
            return View();
        }

        // POST: MaterialSupplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaterialSuplyId,Provider,MaterialId,PriceOfMaterials,AmountOfMaterial,DeliveryDate")] MaterialSupply materialSupply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialSupply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "IdMaterial", "MaterialName", materialSupply.MaterialId);
            _cachedService.RefreshMaterialSupplys();
            return View(materialSupply);
        }

        // GET: MaterialSupplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialSupply = _cachedService.GetMaterialSupply().FirstOrDefault(m => m.MaterialSuplyId == id);
            if (materialSupply == null)
            {
                return NotFound();
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "IdMaterial", "MaterialName", materialSupply.MaterialId);
            return View(materialSupply);
        }

        // POST: MaterialSupplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaterialSuplyId,Provider,MaterialId,PriceOfMaterials,AmountOfMaterial,DeliveryDate")] MaterialSupply materialSupply)
        {
            if (id != materialSupply.MaterialSuplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialSupply);
                    await _context.SaveChangesAsync();
                    _cachedService.RefreshMaterialSupplys();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialSupplyExists(materialSupply.MaterialSuplyId))
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
            ViewData["MaterialId"] = new SelectList(_context.Materials, "IdMaterial", "MaterialName", materialSupply.MaterialId);
            return View(materialSupply);
        }

        // GET: MaterialSupplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialSupply = await _context.MaterialSupplies
                .Include(m => m.Material)
                .FirstOrDefaultAsync(m => m.MaterialSuplyId == id);
            if (materialSupply == null)
            {
                return NotFound();
            }

            return View(materialSupply);
        }

        // POST: MaterialSupplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialSupply = await _context.MaterialSupplies.FindAsync(id);
            _context.MaterialSupplies.Remove(materialSupply);
            await _context.SaveChangesAsync();
            _cachedService.RefreshMaterialSupplys();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialSupplyExists(int id)
        {
            return _context.MaterialSupplies.Any(e => e.MaterialSuplyId == id);
        }
    }
}
