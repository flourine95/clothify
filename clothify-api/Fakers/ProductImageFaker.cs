using Bogus;
using WebApplication1.Models;

namespace WebApplication1.Fakers;

public class ProductImageFaker : Faker<ProductImage>
{
    public ProductImageFaker()
    {
        RuleFor(pi => pi.Image, f => f.Image.PicsumUrl());
        RuleFor(pi => pi.IsMain, false);
    }
}