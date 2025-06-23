using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextEditor.Data;
using TextEditor.Models;

namespace TextEditor.Controllers;

public class EditorController : Controller
{
    private readonly EditorContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EditorController(EditorContext context,  UserManager<IdentityUser> userManager)
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
}