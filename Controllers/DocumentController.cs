using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextEditor.Data;
using TextEditor.Models;

namespace TextEditor.Controllers;
[Authorize]
public class DocumentController : Controller
{
    private readonly EditorContext _context;
    private readonly UserManager<IdentityUser>  _userManager;

    public DocumentController(UserManager<IdentityUser> userManager, EditorContext context)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Document document)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User);
            
            document.UserId = userId;
            
            _context.Add(document);
            
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Home");
        }
        
        return View(document);
    }
    
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var document = await _context.Documents.FindAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        if (document.UserId != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        return View(document);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var document = await _context.Documents.FirstOrDefaultAsync(m=>m.Id == id);

            if (document.UserId != _userManager.GetUserId(User))
            {
                TempData["ErrorMessage"] = "You are not authorized to delete this document.";
                return RedirectToAction("Index", "Home");
            }

            if (document == null)
            {
                TempData["ErrorMessage"] = "Document not found.";
                return RedirectToAction("Index", "Home");
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Document deleted successfully."; 
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return RedirectToAction("Index", "Home");
    }
}