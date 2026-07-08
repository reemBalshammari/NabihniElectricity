using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using NabihniElectricity.Data;
using NabihniElectricity.Models;

namespace NabihniElectricity.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ReportsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // البحث + عرض البلاغات
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.Reports.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x =>
                    x.ReporterName.Contains(searchTerm) ||
                    x.Location.Contains(searchTerm) ||
                    x.ReportType.Contains(searchTerm));
            }

            var model = new ReportViewModel
            {
                Reports = await query.ToListAsync(),
                NewReport = new Report()
            };

            ViewBag.SearchTerm = searchTerm;

            return View(model);
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ReportViewModel model)
{
    if (ModelState.IsValid)
    {
        if (model.NewReport.ImageFile != null)
        {
            var folder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString() +
                           Path.GetExtension(model.NewReport.ImageFile.FileName);

            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.NewReport.ImageFile.CopyToAsync(stream);
            }

            model.NewReport.ImagePath = fileName;
        }

        _context.Reports.Add(model.NewReport);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    model.Reports = await _context.Reports.ToListAsync();

    return View("Index", model);
}

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Reports.FindAsync(id);

            if (report == null) return NotFound();

            return View(report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Report report)
        {
            if (id != report.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(report);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);

            if (report == null) return NotFound();

            return View(report);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}