using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Fakers;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // API Explorer để tích hợp với Swagger
builder.Services.AddSwaggerGen(); // Đăng ký Swagger cho tài liệu OpenAPI

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5174", "http://localhost:5173","http://localhost")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var key = Encoding.ASCII.GetBytes("YourSecretKeyHere"); // Khóa bí mật
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Users.Any())
    {
        var userFaker = new UserFaker();
        var productFaker = new ProductFaker();
        var categoryFaker = new CategoryFaker();
        var brandFaker = new BrandFaker();

        var users = userFaker.Generate(1000);
        await context.Users.AddRangeAsync(users);

        var categories = categoryFaker.Generate(10);
        await context.Categories.AddRangeAsync(categories);

        var brands = brandFaker.Generate(10);
        await context.Brands.AddRangeAsync(brands);

        var products = new List<Product>();
        for (int i = 0; i < 1000; i++)
        {
            products.Add(productFaker.GenerateWithDetails());
        }

        await context.Products.AddRangeAsync(products);

        await context.SaveChangesAsync();
    }
}

// Tích hợp Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = ""; 
    });
}

// Middleware chung
app.UseHttpsRedirection();
app.UseAuthentication(); // Sử dụng Authentication
app.UseAuthorization();  // Sử dụng Authorization
app.UseCors(myAllowSpecificOrigins);

// Định tuyến các controller
app.MapControllers();

app.Run();