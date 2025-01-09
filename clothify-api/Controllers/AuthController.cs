using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace clothify_api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(AppDbContext context, IConfiguration configuration) : ControllerBase
{
    // Đăng ký
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest(new { message = "Email đã tồn tại!" });

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Fullname = string.IsNullOrWhiteSpace(request.Fullname) ? "" : request.Fullname, // Xử lý rỗng
            Phone = string.IsNullOrWhiteSpace(request.Phone) ? "" : request.Phone,  
            Role = 0,
            Status = 1,
            Avatar = "", 
            OauthProvider = "",
            OauthId = "",
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok(new { message = "Đăng ký thành công!" });
    }

    // Đăng nhập
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginUser)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Email == loginUser.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            return Unauthorized("Thông tin đăng nhập không chính xác!");
        // if (user == null || user.Password != loginUser.Password) return Unauthorized("Thông tin đăng nhập không chính xác!");
        // Tạo token
        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    // Tạo JWT Token
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Fullname),
            new Claim(ClaimTypes.Role, user.Role.ToString()) // Thêm Role vào claims
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Fullname { get; set; }
    public string? Phone { get; set; }
}
