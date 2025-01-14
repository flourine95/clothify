namespace clothify_api.Models;

public class DTOUserUpdate
{
    public string Fullname { get; set; }  // Trường Fullname cho tên người dùng
    public string Email { get; set; }     // Trường Email cho địa chỉ email người dùng
    public string Password { get; set; }  // Trường Password để thay đổi mật khẩu
    public string Phone { get; set; }     // Trường Phone cho số điện thoại người dùng
    public int Id { get; set; }
}