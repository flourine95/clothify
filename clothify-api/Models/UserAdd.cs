namespace clothify_api.Models;

public class UserAdd
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string Phone { get; set; }
    public byte Role { get; set; }
    public byte Status { get; set; }
    public string Avatar { get; set; }
    public string OauthProvider { get; set; }
    public string OauthId { get; set; }

}