using Microsoft.EntityFrameworkCore;

namespace clothify_api.Models;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductSale> ProductSales { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<PasswordReset> PasswordResets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostImage> PostImages { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<UserVoucher> UserVouchers { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    
    

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


}

