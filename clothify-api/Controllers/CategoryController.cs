using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothify_api.Controllers;
[ApiController]
[Route("api/categories")]
public class CategoryController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        var categories = await context.Categories.ToListAsync();  // Lấy tất cả các danh mục từ cơ sở dữ liệu

        // Kiểm tra xem có danh mục nào không
        if (!categories.Any())
        {
            return NotFound(); // Trả về 404 nếu không tìm thấy danh mục nào
        }

        // Trả về danh sách danh mục
        return Ok(categories);
    }

    
    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<Category>>> GetLatestCategories([FromQuery] int count = 3)
    {
        // Truy vấn các danh mục mới nhất, sắp xếp theo CreatedAt giảm dần và lấy 6 bản ghi đầu tiên
        var latestCategories = await context.Categories
            .OrderByDescending(c => c.CreatedAt)  // Sắp xếp theo CreatedAt giảm dần
            .Take(count)  // Lấy 6 bản ghi
            .ToListAsync();  // Thực hiện truy vấn bất đồng bộ

        // Kiểm tra xem có danh mục nào không
        if (!latestCategories.Any())
        {
            return NotFound(); // Trả về 404 nếu không tìm thấy danh mục nào
        }

        // Trả về 200 OK cùng với dữ liệu danh mục mới nhất
        return Ok(latestCategories);
    }

}