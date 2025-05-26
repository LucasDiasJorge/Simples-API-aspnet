using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using API;
using API.Models;
using API.Utils;
using API.Utils.DB;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Protects all routes except login
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SecurityService _securityService;

    public UsersController(ApplicationDbContext context, SecurityService securityService)
    {
        _context = context;
        _securityService = securityService;
    }

    // Get all users
    [HttpGet]
    public async Task<ActionResult<ResponseEntity<List<User>>>> GetAllUsers()
    {
        var users = await _context.Users.Include(u => u.CompanyId).ToListAsync();
        return Ok(new ResponseEntity<List<User>>
        {
            Status = "Success",
            Message = "User list retrieved successfully.",
            Data = users
        });
    }

    // Get user by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseEntity<User>>> GetUserById(int id)
    {
        var user = await _context.Users.Include(u => u.CompanyId).FirstOrDefaultAsync(u => u.Id.Equals(id));

        if (user == null)
        {
            return NotFound(new ResponseEntity<User>
            {
                Status = "Error",
                Message = $"User with ID {id} not found.",
                Data = null
            });
        }

        return Ok(new ResponseEntity<User>
        {
            Status = "Success",
            Message = "User found successfully.",
            Data = user
        });
    }

    // Create user
    [HttpPost]
    public async Task<ActionResult<ResponseEntity<User>>> CreateUser([FromBody] User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new ResponseEntity<User>
        {
            Status = "Success",
            Message = "User created successfully.",
            Data = user
        });
    }

    // Update user
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseEntity<User>>> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new ResponseEntity<User>
            {
                Status = "Error",
                Message = $"User with ID {id} not found."
            });
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;
        user.CompanyId = updatedUser.CompanyId;

        await _context.SaveChangesAsync();

        return Ok(new ResponseEntity<User>
        {
            Status = "Success",
            Message = "User updated successfully.",
            Data = user
        });
    }

    // Delete user
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseEntity<User>>> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new ResponseEntity<User>
            {
                Status = "Error",
                Message = $"User with ID {id} not found."
            });
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new ResponseEntity<User>
        {
            Status = "Success",
            Message = "User deleted successfully.",
            Data = user
        });
    }

    // Login method
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<ResponseEntity<object>> Login([FromBody] LoginModel login)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == login.Username);

        if (user != null && _securityService.VerifyPassword(login.Password, user.Password))
        {
            var token = _securityService.GenerateJwtToken(user.Email);
            return Ok(new { token });
        }

        return Unauthorized("Invalid credentials.");
    }
}