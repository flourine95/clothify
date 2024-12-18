using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class ProductVariantFaker : Faker<ProductVariant>
{
    private static readonly string[] Sizes = { "S", "M", "L", "XL", "XXL" };
    private static readonly string[] Colors = { "#FF5733", "#33FF57", "#3357FF", "#F1C40F", "#8E44AD" }; 

    public ProductVariantFaker()
    {
        Locale="vi";
        RuleFor(pv => pv.Size, f => f.PickRandom(Sizes)); 
        RuleFor(pv => pv.Color, f => f.PickRandom(Colors)); 
        RuleFor(pv => pv.Price, f => f.Random.Decimal(1, 100));
        RuleFor(pv => pv.Stock, f => f.Random.Int(0, 50));
        RuleFor(pv => pv.CreatedAt, f => f.Date.Past());
        RuleFor(pv => pv.UpdatedAt, f => f.Date.Recent());
    }
}