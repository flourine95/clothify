using Microsoft.AspNetCore.Mvc;
using clothify_api.Models;
using Microsoft.EntityFrameworkCore;

namespace clothify_api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("add")]
        public IActionResult AddToCart(int productId, int quantity, string size)
        {
            var userId = 1; // Placeholder for actual user ID retrieval
            var cart = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.UserId == userId && o.Status == 0); // Assuming status 0 is 'cart'
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
    
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            if (cart == null)
            {
                cart = new Order { UserId = userId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = 0 };
                _context.Orders.Add(cart);
            }

            // Đảm bảo phương thức AddItem đã có thể xử lý kích thước
            cart.AddItem(product, quantity, size); // Thêm tham số size vào đây
            _context.SaveChanges();

            return Ok(cart);
        }


        [HttpPost("remove")]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = 1; // Placeholder for actual user ID retrieval
            var cart = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.UserId == userId && o.Status == 0);
            if (cart == null)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }

            cart.RemoveItem(productId);
            _context.SaveChanges();

            return Ok(cart);
        }

        [HttpGet]
        public IActionResult GetCart()
        {
            var userId = 1; // Placeholder for actual user ID retrieval
            var cart = _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                                      .FirstOrDefault(o => o.UserId == userId && o.Status == 0);
            if (cart == null)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }

            return Ok(cart);
        }
    }
}