using Bogus;
using WebApplication1.Models;

namespace WebApplication1.Fakers;

public class BrandFaker : Faker<Brand>
{
    public BrandFaker()
    {
        RuleFor(b => b.Name, f => f.Company.CompanyName());
        RuleFor(b => b.Image, f => f.Image.PicsumUrl());
        RuleFor(b => b.Description, f => f.Lorem.Sentence());
        RuleFor(b => b.CreatedAt, f => f.Date.Past());
        RuleFor(b => b.UpdatedAt, f => f.Date.Recent());
    }
}