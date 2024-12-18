using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class ProductImageFaker : Faker<ProductImage>
{
    public ProductImageFaker()
    {
        Locale="vi";
        RuleFor(pi => pi.Image, f => f.Image.PicsumUrl());
        RuleFor(pi => pi.IsMain, false);
    }
}