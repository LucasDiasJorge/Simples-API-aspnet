using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Company
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [StringLength(64)]
    public string Name { get; set; }

    public Company(string name)
    { 
        Name = name;
    }

}