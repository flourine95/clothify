namespace WebApplication1.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int TotalViews { get; set; }
    public bool IsFeatured { get; set; }
    public byte Status { get; set; }
    public decimal AverageRating { get; set; }
    public string Slug { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Category Category { get; set; }
    public Brand Brand { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    public ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();
    public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}