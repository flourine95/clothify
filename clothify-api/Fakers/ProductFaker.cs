using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        Locale="vi";
        RuleFor(p => p.Name, f => f.Commerce.ProductName());
        RuleFor(p => p.Description, f => f.Lorem.Paragraph());
        RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price())); // Đảm bảo kiểu dữ liệu là decimal
        RuleFor(p => p.Stock, f => f.Random.Int(1, 100));
        RuleFor(p => p.TotalViews, f => f.Random.Int(0, 1000));
        RuleFor(p => p.IsFeatured, f => f.Random.Bool());
        RuleFor(p => p.Status, f => f.Random.Byte(0, 1));
        RuleFor(p => p.AverageRating, f => f.Random.Decimal(1, 5));
        RuleFor(p => p.Slug, f => f.Lorem.Slug());
        RuleFor(p => p.CategoryId, f => f.Random.Int(1, 10)); 
        RuleFor(p => p.BrandId, f => f.Random.Int(1, 10)); 
        RuleFor(p => p.CreatedAt, f => f.Date.Past());
        RuleFor(p => p.UpdatedAt, f => f.Date.Recent());
    }

    public Product GenerateWithDetails()
    {
        var product = Generate();

        var productImageFaker = new ProductImageFaker();
        var productReviewFaker = new ProductReviewFaker();
        var productVariantFaker = new ProductVariantFaker();

        var images = productImageFaker.Generate(4);
        foreach (var image in product.ProductImages)
        {
            image.ProductId = product.Id;
            image.IsMain = true;
        }

        var mainImageIndex = new Random().Next(0, images.Count); 
        images[mainImageIndex].IsMain = true;
        product.ProductImages = images;


        product.ProductReviews = productReviewFaker.Generate(3);
        foreach (var review in product.ProductReviews)
        {
            review.ProductId = product.Id;
        }


        product.ProductVariants = productVariantFaker.Generate(5);
        foreach (var variant in product.ProductVariants)
        {
            variant.ProductId = product.Id;
        }

        return product;
    }
}