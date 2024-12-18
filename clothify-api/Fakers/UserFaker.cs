using Bogus;
using WebApplication1.Models;

namespace WebApplication1.Fakers;

public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(u => u.Email, f => f.Internet.Email());
        RuleFor(u => u.Password, f => f.Internet.Password(8, true)); 
        RuleFor(u => u.Fullname, f => f.Name.FullName());
        RuleFor(u => u.Phone, f => f.Phone.PhoneNumber());
        RuleFor(u => u.Role, f => f.Random.Byte(0, 1)); 
        RuleFor(u => u.Status, f => f.Random.Byte(0, 1)); 
        RuleFor(u => u.Avatar, f => f.Internet.Avatar());
        RuleFor(u => u.OauthProvider, f => f.Internet.DomainName());
        RuleFor(u => u.OauthId, f => f.Random.Guid().ToString());
        RuleFor(u => u.EmailVerified, f => f.Random.Bool());
        RuleFor(u => u.CreatedAt, f => f.Date.Past());
        RuleFor(u => u.UpdatedAt, f => f.Date.Recent());
    }
}