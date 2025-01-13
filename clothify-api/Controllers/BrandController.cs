using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothify_api.Controllers;
[ApiController]
[Route("api/brands")]
public class BrandController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
    {
        var brands = await context.Brands.ToListAsync();
        return Ok(brands);
    }
}
