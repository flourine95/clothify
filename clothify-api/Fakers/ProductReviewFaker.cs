using Bogus;
using WebApplication1.Models;

namespace WebApplication1.Fakers;

public class ProductReviewFaker : Faker<ProductReview>
{
    private static readonly string[] ReviewTexts =
    [
        "The fabric is soft and comfortable, perfect for everyday wear.",
        "I love the fit of this shirt, it’s stylish and flattering.",
        "Great quality! The stitching is very neat, and it feels durable.",
        "The color of the jacket is just like the picture, very vibrant!",
        "These jeans are true to size and fit perfectly, highly recommend.",
        "I’m impressed by how quickly it dries, great for workouts.",
        "This dress is so elegant and perfect for any occasion.",
        "The hoodie is warm and cozy, just what I needed for the winter.",
        "I love the stretchy material, it’s so comfortable and doesn’t lose shape.",
        "The material feels premium, definitely worth the price.",
        "Perfect for casual outings, and the design is modern and trendy.",
        "This t-shirt is really breathable and feels light on the skin.",
        "I bought these shoes for a formal event, and they are absolutely perfect.",
        "The shorts fit well, and they’re great for hot summer days.",
        "I’m very happy with my purchase, the quality exceeded my expectations.",
        "The jacket is well insulated, keeping me warm in colder weather.",
        "This sweater is soft, warm, and perfect for layering in the fall.",
        "The fabric has a nice stretch, making it perfect for any activity.",
        "I was looking for a comfortable but stylish pair of pants, and these are it!",
        "Absolutely love these boots! They are stylish and very comfortable."
    ];

    public ProductReviewFaker()
    {
        RuleFor(pr => pr.UserId, f => f.Random.Int(1, 1000));
        RuleFor(pr => pr.Rating, f => f.Random.Byte(1, 5));
        RuleFor(pr => pr.ReviewText, f => f.PickRandom(ReviewTexts));
        RuleFor(pr => pr.IsVerified, f => f.Random.Bool());
        RuleFor(pr => pr.HelpfulVotes, f => f.Random.Int(0, 100));
        RuleFor(pr => pr.CreatedAt, f => f.Date.Past());
        RuleFor(pr => pr.UpdatedAt, f => f.Date.Recent());
    }
}