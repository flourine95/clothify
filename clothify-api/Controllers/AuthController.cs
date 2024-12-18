using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace clothify_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext context) : ControllerBase
{
    private readonly string _secretKey = "YourSecretKeyHere"; // Khóa bí mật

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = context.Users.SingleOrDefault(u => u.Email == request.Email && u.Password == request.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}