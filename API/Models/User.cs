using System.ComponentModel.DataAnnotations;

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
    
    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}

