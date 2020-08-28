using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VueCliMiddleware;
using System.IO;

namespace TCGStreamHelper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Add AddRazorPages if the app uses Razor Pages.
            services.AddRazorPages();

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                if (env.IsDevelopment())
                {
                    endpoints.MapToVueCliProxy(
                        "{*path}",
                        new SpaOptions { SourcePath = "ClientApp" },
                        npmScript: "serve",
                        regex: "Compiled successfully");
                }

                // Add MapRazorPages if the app uses Razor Pages. Since Endpoint Routing includes support for many frameworks, adding Razor Pages is now opt -in.
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });

            try{
                if (!Directory.Exists("activeImage")) Directory.CreateDirectory("activeImage");
                if (!Directory.Exists("wwwroot")) Directory.CreateDirectory("wwwroot");
                if (!Directory.Exists("wwwroot/cards")) Directory.CreateDirectory("wwwroot/cards");
                if (!Directory.Exists("players")) Directory.CreateDirectory("players");

                if(!File.Exists("players/playerLeft_name.txt")) File.Create("players/playerLeft_name.txt").Close(); 
                if(!File.Exists("players/playerLeft_deck.txt")) File.Create("players/playerLeft_deck.txt").Close();
                if(!File.Exists("players/playerLeft_score.txt")) File.Create("players/playerLeft_score.txt").Close();
                if(!File.Exists("players/playerLeft_lpoints.txt")) File.Create("players/playerLeft_lpoints.txt").Close();
                if(!File.Exists("players/playerRight_name.txt")) File.Create("players/playerRight_name.txt").Close();
                if(!File.Exists("players/playerRight_deck.txt")) File.Create("players/playerRight_deck.txt").Close();
                if(!File.Exists("players/playerRight_score.txt")) File.Create("players/playerRight_score.txt").Close();
                if(!File.Exists("players/playerRight_lpoints.txt")) File.Create("players/playerRight_lpoints.txt").Close();

            }
            catch(System.Exception e)
            {
                logger.LogError($"Folders or files could not be created at startup :\n{e}");
            }
        }
    }
}
