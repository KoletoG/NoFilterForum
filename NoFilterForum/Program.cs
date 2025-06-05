using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Ganss.Xss;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;
using NoFilterForum.Infrastructure.Repositories;
using NoFilterForum.Infrastructure.Services;
using NoFilterForum.Interfaces;
using NoFilterForum.Services;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<UserDataModel>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = true;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IIOService, IOService>();
            builder.Services.AddScoped<IHtmlSanitizer, HtmlSanitizer>();
            builder.Services.AddScoped<INonIOService, NonIOService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IWarningRepository, WarningRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
            builder.Services.AddScoped<ISectionRepository, SectionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISectionService, SectionService>();
            builder.Services.AddScoped<IReplyService, ReplyService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IWarningService, WarningService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPostFactory,PostFactory>();
            builder.Services.AddScoped<ISectionFactory, SectionFactory>();
            builder.Services.AddScoped<IReportFactory, ReportFactory>();
            builder.Services.AddScoped<IReplyFactory, ReplyFactory>();
            builder.Services.ConfigureApplicationCookie(c =>
            {
                c.Cookie.HttpOnly = true;
                c.Cookie.SameSite = SameSiteMode.Strict;
                c.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                c.SlidingExpiration = true;
                c.ExpireTimeSpan = TimeSpan.FromDays(14);
                c.Cookie.MaxAge = TimeSpan.FromDays(14);
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets(); 
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Section}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
