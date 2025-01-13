using clothify_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdmin : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserAdmin(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAdmin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(int page = 1, int pageSize = 10)
        {
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/UserAdmin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"Người dùng với Id {id} không tồn tại.");
            }

            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] UserAdd userAdd)
        {
            // Kiểm tra thông tin bắt buộc
            if (string.IsNullOrEmpty(userAdd.Email) || string.IsNullOrEmpty(userAdd.Fullname))
            {
                return BadRequest("Email và Fullname là bắt buộc.");
            }

            // Kiểm tra trùng email
            if (_context.Users.Any(u => u.Email == userAdd.Email))
            {
                return Conflict("Email đã tồn tại.");
            }

            // Tạo đối tượng User từ UserAdd
            var user = new User
            {
                Email = userAdd.Email,
                Password = userAdd.Password, // Bạn có thể thêm mã hóa mật khẩu tại đây nếu cần
                Fullname = userAdd.Fullname,
                Phone = userAdd.Phone,
                Role = userAdd.Role,
                Status = userAdd.Status,
                Avatar = userAdd.Avatar,
                OauthProvider = userAdd.OauthProvider,
                OauthId = userAdd.OauthId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Trả về thông tin người dùng đã thêm
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdate userUpdate)
        {
            if (userUpdate == null)
            {
                return BadRequest("Dữ liệu người dùng không hợp lệ.");
            }

            // Kiểm tra xem ID trong URL có khớp với ID trong body
            if (id != userUpdate.Id)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound($"Không tìm thấy người dùng với ID {id}.");
            }

            // Cập nhật các trường nếu có dữ liệu
            if (!string.IsNullOrEmpty(userUpdate.Fullname))
            {
                existingUser.Fullname = userUpdate.Fullname;
            }

            if (!string.IsNullOrEmpty(userUpdate.Email))
            {
                existingUser.Email = userUpdate.Email;
            }

            if (!string.IsNullOrEmpty(userUpdate.Password))
            {
                existingUser.Password = userUpdate.Password; // Bạn có thể mã hóa mật khẩu ở đây nếu cần
            }

            if (!string.IsNullOrEmpty(userUpdate.Phone))
            {
                existingUser.Phone = userUpdate.Phone;
            }

            // Cập nhật thời gian sửa đổi
            existingUser.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Có lỗi xảy ra khi cập nhật người dùng.");
            }

            return NoContent(); // Trả về trạng thái 204 No Content khi cập nhật thành công
        }

        // DELETE: api/UserAdmin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (_context.Orders.Any(o => o.UserId == id))
            {
                return BadRequest("Không thể xóa người dùng này vì họ có đơn hàng.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}