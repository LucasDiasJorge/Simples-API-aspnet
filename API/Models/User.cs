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
    
    [ForeignKey("CompanyId")]
    public Guid CompanyId { get; set; }

    
    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}

