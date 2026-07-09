using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(
    AppDbContext context,
    IWebHostEnvironment environment,
    UserManager<ApplicationUser> userManager)
{
    _context = context;
    _environment = environment;
    _userManager = userManager;
}

        // البحث + عرض البلاغات
       public async Task<IActionResult> Index(string searchTerm)
{
    var currentUser = await _userManager.GetUserAsync(User);

    IQueryable<Report> query;

    if (User.IsInRole("Admin"))
    {
        query = _context.Reports;
    }
    else
    {
        query = _context.Reports.Where(r => r.UserId == currentUser!.Id);
    }

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
var currentUser = await _userManager.GetUserAsync(User);
model.NewReport.UserId = currentUser!.Id;
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
            var currentUser = await _userManager.GetUserAsync(User);

if (report == null)
    return NotFound();

if (!User.IsInRole("Admin") && report.UserId != currentUser!.Id)
{
    return Forbid();
}

            return View(report);
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Report report)
{
    if (id != report.Id)
        return NotFound();

    var oldReport = await _context.Reports
        .FirstOrDefaultAsync(r => r.Id == id);

    if (oldReport == null)
        return NotFound();

    if (!User.IsInRole("Admin") && oldReport.UserId != report.UserId)
        return Forbid();

    // تحديث البيانات
    oldReport.ReporterName = report.ReporterName;
    oldReport.Phone = report.Phone;
    oldReport.Location = report.Location;
    oldReport.ReportType = report.ReportType;
    oldReport.Description = report.Description;

    // الأدمن فقط يغير الحالة
    if (User.IsInRole("Admin"))
    {
        if (oldReport.Status != report.Status)
        {
            string statusAr = report.Status;
string statusEn = report.Status;

switch (report.Status)
{
    case "قيد الانتظار":
        statusEn = "Pending";
        break;

    case "قيد المعالجة":
        statusEn = "In Progress";
        break;

    case "تمت المعالجة":
        statusEn = "Completed";
        break;

    case "رُفضت المعالجة":
        statusEn = "Rejected";
        break;
}
           var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

_context.Notifications.Add(new Notification
{
    UserId = oldReport.UserId!,
    Status = report.Status,
    CreatedAt = DateTime.Now,
    IsRead = false
});
        }

        oldReport.Status = report.Status;
    }

    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);
            var currentUser = await _userManager.GetUserAsync(User);

if (report == null)
    return NotFound();

if (!User.IsInRole("Admin") && report.UserId != currentUser!.Id)
{
    return Forbid();
}

            return View(report);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);

if (report == null)
    return NotFound();

if (!User.IsInRole("Admin") && report.UserId != currentUser!.Id)
{
    return Forbid();
}

            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}