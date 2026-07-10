using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NabihniElectricity.Data;
using NabihniElectricity.Models;

namespace NabihniElectricity.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public HomeController(
        UserManager<ApplicationUser> userManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactMessage model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.ContactMessages.Add(model);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إرسال رسالتك بنجاح";

        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Services()
    {
        return View();
    }

    public IActionResult ReportTypes()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(ApplicationUser model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;

        await _userManager.UpdateAsync(user);

        TempData["Success"] = "تم تحديث البيانات بنجاح";

        return RedirectToAction(nameof(Profile));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}