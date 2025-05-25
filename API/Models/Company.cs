using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Company
{
    [Key] 
    public int ID { get; set; }
    
    public string Name { get; set; }

    public Company(int id, string name)
    {
        ID = id;
        Name = name;
    }

}