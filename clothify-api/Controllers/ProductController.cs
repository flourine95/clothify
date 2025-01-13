using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothify_api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(AppDbContext context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] bool? isFeatured = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? startsWith = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] int? brandId = null,
        [FromQuery] string? status = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? search = null, // Thêm tham số tìm kiếm
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 9)
    {
        var query = context.Set<Product>()
            .Where(p => p.Status != 1)
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.TotalViews,
                p.Category,
                p.IsFeatured,
                p.CategoryId,
                p.BrandId,
                p.CreatedAt,
                MainImage = p.ProductImages.FirstOrDefault(img => img.IsMain),
                AverageRating = context.ProductReviews.Where(r => r.ProductId == p.Id).Average(r => (double?)r.Rating) ?? 0,
                ReviewCount = context.ProductReviews.Count(r => r.ProductId == p.Id),
                SalePercentage = context.ProductSales
                    .Where(ps => ps.ProductId == p.Id &&
                                 ps.Sale.StartDate <= DateTime.Now &&
                                 ps.Sale.EndDate >= DateTime.Now)
                    .Select(ps => ps.Sale.DiscountPercentage)
                    .FirstOrDefault(),
                OriginalPrice = p.Price
            })
            .AsQueryable();

        // Thêm điều kiện tìm kiếm
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search)); // Tìm kiếm theo tên sản phẩm
        }

        // Áp dụng bộ lọc nếu có
        if (isFeatured.HasValue)
            query = query.Where(p => p.IsFeatured == isFeatured.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(startsWith))
            query = query.Where(p => p.Name.Contains(startsWith)); // Tìm kiếm theo tên sản phẩm

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value); // Lọc theo thương hiệu

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p =>
                p.Stock > 0 && status == "in-stock" || p.Stock == 0 && status == "out-of-stock"); // Lọc theo trạng thái

        // Sắp xếp theo tiêu chí
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "priceasc" => query.OrderBy(p => p.Price * p.SalePercentage),
                "pricedesc" => query.OrderByDescending(p => p.Price * p.SalePercentage),
                "name" => query.OrderBy(p => p.Name),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                "rating" => query.OrderByDescending(p => p.AverageRating) // Giả sử bạn có thuộc tính AverageRating
                ,
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedAt); // Mặc định sắp xếp theo ngày tạo
        }

        // Tổng số lượng sản phẩm
        var totalItems = await query.CountAsync();

        // Phân trang
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
        Response.Headers.Append("X-Total-Count", totalItems.ToString());
        Response.Headers.Append("Access-Control-Allow-Origin", "*");

        return Ok(products);
    }


    [HttpGet("random")]
    public async Task<IActionResult> GetRandomProducts([FromQuery] int count = 6)
    {
        var randomProducts = await context.Products
            .OrderBy(p => Guid.NewGuid()) // Sắp xếp ngẫu nhiên bằng GUID
            .Take(count) // Giới hạn số lượng sản phẩm trả về
            .Select(p => new
            {
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
            .Select(p => new
            {
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
            .Select(p => new
            {
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