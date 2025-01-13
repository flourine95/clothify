using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothify_api.Controllers;
[ApiController]
[Route("api/categories")]
public class CategoryController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await context.Categories.ToListAsync();
        return Ok(categories);
    }

}