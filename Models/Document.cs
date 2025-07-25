using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TextEditor.Models;

public class Document
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    [Required]
    public string? UserId { get; set; }
    
    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }
}