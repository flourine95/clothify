using Bogus;
using clothify_api.Models;

namespace clothify_api.Fakers
{
    public class SaleFaker : Faker<Sale>
    {
        public SaleFaker()
        {
            Locale = "vi";
            RuleFor(s => s.Name, f => f.Commerce.ProductName());
            RuleFor(s => s.Description, f => f.Lorem.Paragraph());
            RuleFor(s => s.DiscountPercentage, f => f.Random.Decimal(5, 50)); // Giảm giá từ 5% đến 50%
            RuleFor(s => s.StartDate, f => f.Date.Past(1)); // Ngày bắt đầu trong quá khứ
            RuleFor(s => s.EndDate, f => f.Date.Future(1)); // Ngày kết thúc trong tương lai
            RuleFor(s => s.CreatedAt, f => f.Date.Past());
            RuleFor(s => s.UpdatedAt, f => f.Date.Recent());
        }
    }
}