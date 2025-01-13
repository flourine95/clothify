using Microsoft.AspNetCore.Mvc;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDistributedMemoryCache(); // Cấu hình bộ nhớ cache cho session
        services.AddSession(options => 
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSession(); // Sử dụng session trong ứng dụng
        app.UseRouting();
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}