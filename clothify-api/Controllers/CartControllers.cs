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
        
        
        // Thêm sản phẩm vào giỏ
        [HttpPost("add")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = 1; // ID của người dùng (có thể lấy từ token hoặc session)
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            cart.AddItem(product, quantity);
            _context.SaveChanges();

            return Ok(cart);
        }

        // Xóa sản phẩm khỏi giỏ
        [HttpPost("remove")]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = 1; // ID của người dùng
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }

            cart.RemoveItem(productId);
            _context.SaveChanges();

            return Ok(cart);
        }

        // Lấy giỏ hàng của người dùng
        [HttpGet]
        public IActionResult GetCart()
        {
            var userId = 1; // ID người dùng
            var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                                     .FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }

            return Ok(cart);
        }
    }
}
