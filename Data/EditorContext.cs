using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TextEditor.Models;

namespace TextEditor.Data;

public class EditorContext : IdentityDbContext
{
    public EditorContext(DbContextOptions<EditorContext> options) : base(options)
    {
    }
    
    public DbSet<Document> Documents { get; set; }
}