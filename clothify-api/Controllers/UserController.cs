using clothify_api.Models;

namespace clothify_api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController(AppDbContext context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }
}