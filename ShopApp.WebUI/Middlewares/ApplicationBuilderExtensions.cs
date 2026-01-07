using Microsoft.Extensions.FileProviders;

namespace ShopApp.WebUI.Middlewares;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder CustomStaticfiles(this IApplicationBuilder app)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "node_modules");
        
        if (Directory.Exists(path))
        {
            var options = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/modules"
            };
            app.UseStaticFiles(options);
        }
        
        return app;
    }
}
