using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class CategoryFaker : Faker<Category>
{
    public CategoryFaker()
    {
        Locale="vi";
        RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
        RuleFor(c => c.Image, f => f.Image.PicsumUrl());
        RuleFor(c => c.Description, f => f.Lorem.Sentence());
        RuleFor(c => c.CreatedAt, f => f.Date.Past());
        RuleFor(c => c.UpdatedAt, f => f.Date.Recent());
    }
}