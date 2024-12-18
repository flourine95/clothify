namespace clothify_api.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string Phone { get; set; }
    public byte Role { get; set; }
    public byte Status { get; set; }
    public string Avatar { get; set; }
    public string OauthProvider { get; set; }
    public string OauthId { get; set; }
    public bool EmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
}