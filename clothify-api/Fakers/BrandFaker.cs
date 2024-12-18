using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class BrandFaker : Faker<Brand>
{
    public BrandFaker()
    {
        Locale="vi";
        RuleFor(b => b.Name, f => f.Company.CompanyName());
        RuleFor(b => b.Image, f => f.Image.PicsumUrl());
        RuleFor(b => b.Description, f => f.Lorem.Sentence());
        RuleFor(b => b.CreatedAt, f => f.Date.Past());
        RuleFor(b => b.UpdatedAt, f => f.Date.Recent());
    }
}