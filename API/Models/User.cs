using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class User
{

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [StringLength(64)]
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public Company Company { get; set; }

}

