﻿using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers;

public class ProductSaleFaker : Faker<ProductSale>
{
    public ProductSaleFaker()
    {
        Locale="vi";
        RuleFor(ps => ps.ProductId, f => f.Random.Int(1, 100)); // Giả định có 100 sản phẩm
        RuleFor(ps => ps.SaleId, f => f.Random.Int(1, 10)); // Giả định có 10 chương trình khuyến mãi
        RuleFor(ps => ps.CreatedAt, f => f.Date.Past());
        RuleFor(ps => ps.UpdatedAt, f => f.Date.Recent());
    }
}