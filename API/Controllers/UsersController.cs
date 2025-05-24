using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using API;
using API.Models;
using API.Utils;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Protects all routes except login
public class UsersController : ControllerBase
{
    private SecurityService _securityService;

    public UsersController(SecurityService securityService)
    {
        _securityService = securityService;
    }

    private static List<User> users = new List<User>
    {
        new User { Id = 1, Name = "Alice", Email = "alice@example.com", Password = "alicestrongpass" },
        new User { Id = 2, Name = "Bob", Email = "bob@example.com", Password = "bobstrongpass" }
    };

    // CRUD Operations

    [HttpGet]
    public ActionResult<ResponseEntity> GetAllUsers()
    {
        var response = new ResponseEntity()
        {
            Status = "Success",
            Message = "This is a sample response entity.",
            Data = users
        };
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<ResponseEntity> GetUserById(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
    
        if (user == null)
        {
            return NotFound(new ResponseEntity
            {
                Status = "Error",
                Message = $"User with ID {id} not found.",
                Data = null
            });
        }

        var response = new ResponseEntity
        {
            Status = "Success",
            Message = "User found successfully.",
            Data = user
        };

        return Ok(response);
    }



    [HttpPost]
    public ActionResult<ResponseEntity> CreateUser([FromBody] User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);

        var response = new ResponseEntity()
        {
            Status = "Success",
            Message = "User created successfully.",
            Data = user
        };
        
        return CreatedAtAction(nameof(GetUserById),response);
    }

    [HttpPut("{id}")]
    public ActionResult<ResponseEntity> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {

            var response = new ResponseEntity()
            {
                Status = "Error",
                Message = $"User with ID {id} not found.",
            };
            
            return NotFound(response);
        
        }
        else
        {
            
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            var response = new ResponseEntity()
            {
                Status = "Success",
                Message = "User updated successfully.",
                Data = user
            };
            
            return Ok(response);

        }
    }

    [HttpDelete("{id}")]
    public ActionResult<ResponseEntity> DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {

            var response = new ResponseEntity()
            {
                Status = "Error",
                Message = $"User with ID {id} not found.",
            };
            
            return NotFound(response);
        
        }
        else
        {
            users.Remove(user);

            var response = new ResponseEntity()
            {
                Status = "Success",
                Message = "User deleted successfully.",
                Data = user
            };
            return (response);
        }
    }

    // Login Method
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<ResponseEntity> Login([FromBody] LoginModel login)
    {
        var user = users.FirstOrDefault(u => u.Email == login.Username);

        if (user != null && _securityService.VerifyPassword(login.Password, user.Password))
        {
            var token = _securityService.GenerateJwtToken(user.Email);
            return Ok(new { token });
        }

        return Unauthorized("Invalid credentials.");
    }
}