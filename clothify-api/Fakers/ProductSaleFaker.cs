using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class ProductSaleFaker : Faker<ProductSale>
{
    public ProductSaleFaker()
    {
        Locale = "vi";
        RuleFor(ps => ps.ProductId, f => f.Random.Int(1, 1000)); // Giả sử bạn có từ 1 đến 1000 sản phẩm trong bảng Products
        RuleFor(ps => ps.SaleId, f => f.Random.Int(1, 10));
        RuleFor(ps => ps.CreatedAt, f => f.Date.Past());
        RuleFor(ps => ps.UpdatedAt, f => f.Date.Recent());
    }
}