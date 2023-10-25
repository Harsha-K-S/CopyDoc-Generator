using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;

namespace WebTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                builder.Logging.AddSerilog(Log.Logger);
                builder.Services.AddSingleton(s => new HtmlStore(builder.Configuration.GetValue<string>("Directory")));
                builder.Services.AddSingleton(s => new RequestStore(builder.Configuration.GetValue<string>("Requests")));
                builder.Services.AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
                builder.Services.AddSingleton<IUserStore<ApplicationUser>>(s => new ApplicationUserStore(builder.Configuration.GetValue<string>("Users"), s.GetRequiredService<IPasswordHasher<ApplicationUser>>()));
                builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();

                builder.Services
                    .AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddDefaultTokenProviders()
                    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

                builder.Services.AddRazorPages();
                builder.Services.AddControllersWithViews(/*options => options.SuppressAsyncSuffixInActionNames = false*/);

                WebApplication app = builder.Build();

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                //app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthorization();
                app.MapControllerRoute(name: "default", pattern: "{controller=Users}/{action=Dashboard}");
                app.MapRazorPages();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}