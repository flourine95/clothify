using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothify_api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .ToListAsync();
    }
    
    [HttpGet("random")]
    public async Task<IActionResult> GetRandomProducts([FromQuery] int count = 6)
    {
        var randomProducts = await context.Products
            .OrderBy(p => Guid.NewGuid()) // Sắp xếp ngẫu nhiên bằng GUID
            .Take(count) // Giới hạn số lượng sản phẩm trả về
            .Select(p => new {
                p.Id,
                p.Name,
                p.Price,
                ImageUrl = p.ProductImages.FirstOrDefault()!.Image // Lấy ảnh đầu tiên của sản phẩm
            })
            .ToListAsync();

        return Ok(randomProducts);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedProducts([FromQuery] int count = 3)
    {
        var products = await context.Products
            .Where(p => p.IsFeatured) // Lọc sản phẩm nổi bật
            .OrderByDescending(p => p.CreatedAt) // Sắp xếp theo ngày tạo mới nhất
            .Take(count) // Giới hạn số lượng sản phẩm trả về
            .Include(p => p.ProductImages) // Bao gồm hình ảnh sản phẩm
            .Select(p => new {
                p.Id,
                p.Name,
                p.Price,
                 p.AverageRating, // Đánh giá trung bình
                ReviewCount = context.ProductReviews.Count(r => r.ProductId == p.Id), // Đếm số lượng đánh giá
                p.Description,
                ImageUrl = p.ProductImages.FirstOrDefault() // Ảnh sản phẩm
            })
            .ToListAsync();

        return Ok(products);
    }



    
    [HttpGet("new-arrivals")]
    public async Task<IActionResult> GetNewProducts([FromQuery] int count = 5)
    {
        var products = await context.Products
            .OrderByDescending(p => p.CreatedAt) // Sắp xếp theo ngày tạo mới nhất
            .Take(count) // Giới hạn số lượng sản phẩm trả về
            .Select(p => new {
                p.Id,
                p.Name,
                p.Price,
                p.CreatedAt,
                ImageUrl = p.ProductImages.FirstOrDefault()!.Image 
            })
            .ToListAsync();

        return Ok(products);
    }


    

    


    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        context.Products.Add(product);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        product.UpdatedAt = DateTime.UtcNow;
        context.Entry(product).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id);
    }
}