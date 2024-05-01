using Microsoft.AspNetCore.Mvc;
using TestApi.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthContext _context;
        private readonly List<string> _invalidTokens; // Blacklist of invalidated tokens

        public LoginController(AuthContext context)
        {
            _context = context;
            _invalidTokens = new List<string>();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            // Validate the model
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null || user.Password != model.Password)
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return the token in the response
            return Ok(new { token ,user});
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty), // Use string.Empty if user.Name is null
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty), // Use string.Empty if user.Email is null
         // Add more claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here!firasba123anis123"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your_issuer_here",
                audience: "your_audience_here",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Adjust expiration time as needed
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Assuming you receive the token to invalidate from the client
            var tokenToInvalidate = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            _invalidTokens.Add(tokenToInvalidate); // Add the token to the blacklist

            return Ok("Logout successful");
        }

        // Add other actions as needed
    }
}
