﻿using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group10_WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<User> _signInManager;

        public AuthController(AppDbContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                // Check if a user with the same email already exists
                var existingUser = await _context.Users
                                                  .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    // Return a conflict response if the user already exists
                    return Conflict(new { message = "A user with this email already exists" });
                }

                // If no duplicate user is found, add the new user
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Return a success response
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                // If something goes wrong, return a BadRequest with the error message
                return BadRequest(new { message = $"Error during registration: {ex.Message}" });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            if (foundUser == null) return Unauthorized("Invalid credentials");

            foundUser.UserName = foundUser.Email;
            // You can set the user as logged in using the SignInManager if needed
            await _signInManager.SignInAsync(foundUser, isPersistent: false);

            return Ok(foundUser);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  // Sign the user out
            return RedirectToAction("Index", "Home");  // Redirect to Home page, or wherever you'd like
        }


        [HttpGet("users/all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null) return NotFound();
            return users;
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost("editUser/{id}")]
        [Authorize]
        public async Task<IActionResult> EditUser(int id, [FromBody] User user)
        {

            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existingUser == null)
            {
                return NotFound();  // Return NotFound if user doesn't exist
            }

            // Update user details
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;  // Update fields accordingly
            
            _context.Entry(existingUser).State = EntityState.Modified;
            // EF might try to track all fields, make sure we don't mark UserId for update
            _context.Entry(existingUser).Property(u => u.UserId).IsModified = false;
            await _context.SaveChangesAsync();
            return NoContent();  // Successfully updated
        }


        [HttpPost("deleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
