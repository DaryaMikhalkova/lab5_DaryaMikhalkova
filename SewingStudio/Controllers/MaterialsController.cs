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
    public class MaterialsController : Controller
    {
        private readonly AtelierContext _context;
        private readonly CachedService _cachedService;
        private readonly int _pSize = 20;

        public MaterialsController(AtelierContext context, CachedService cachedService)
        {
            _context = context;
            _cachedService = cachedService;
        }

        // GET: Materials
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            page--;
            var cnt = _context.Materials.Count();
            return View(new DataViewModel<Material>()
            {
                Data = _cachedService.GetMaterials().Skip(page * _pSize).Take(_pSize),
                PageCount = cnt / _pSize + (cnt % _pSize > 0 ? 1 : 0),
                CurrentPage = page
            });
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = _cachedService.GetMaterials()
                .FirstOrDefault(m => m.IdMaterial == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMaterial,MaterialName,MaterialType,QuantityMaterialInStock")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                _cachedService.RefreshMaterials();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = _cachedService.GetMaterials().FirstOrDefault(m => m.IdMaterial == id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMaterial,MaterialName,MaterialType,QuantityMaterialInStock")] Material material)
        {
            if (id != material.IdMaterial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                    _cachedService.RefreshMaterials();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.IdMaterial))
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
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.IdMaterial == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            _cachedService.RefreshMaterials();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.IdMaterial == id);
        }
    }
}
