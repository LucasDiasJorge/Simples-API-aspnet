using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class User
{

    [Key]
    public int ID { get; set; } // Primary key
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    [Key]
    [Required]
    public Company Company { get; set; }
    
    public User(int id, string name, string email, string password, Company company)
    {
        ID = id;
        Name = name;
        Email = email;
        Password = password;
        Company = company;
    }
    
}

