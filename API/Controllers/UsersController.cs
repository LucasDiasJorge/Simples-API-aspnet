using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using API;
using API.Models;

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
    public IActionResult GetAllUsers()
    {
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        users.Remove(user);
        return NoContent();
    }

    // Login Method
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
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