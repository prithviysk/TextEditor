using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextEditor.Data;
using TextEditor.Models;

namespace TextEditor.Controllers;

public class HomeController : Controller
{
    private readonly EditorContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(EditorContext context,  UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        List<Document> documents = new List<Document>();
        
        if (User.Identity!.IsAuthenticated)
        {
            var currentUserId = _userManager.GetUserId(User);
            documents = await _context.Documents.Include(d => d.User).Where(d=>d.UserId == currentUserId).ToListAsync();
        }
        
        return View(documents);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}