using Application.Implementations.Services;
using Application.Interfaces.Services;
using Core.Implementations.Services;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Ganss.Xss;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.SignalRHubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;
using NoFilterForum.Infrastructure.Repositories;
using NoFilterForum.Infrastructure.Services;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Regular", policy => policy.RequireRole("Regular"));
                options.AddPolicy("VIP", policy => policy.RequireRole("VIP"));
                options.AddPolicy("Dinosaur", policy => policy.RequireRole("Dinosaur"));
                options.AddPolicy("Newbie", policy => policy.RequireRole("Newbie"));
                options.AddPolicy("Deleted", policy => policy.RequireRole("Deleted"));
            });
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IHtmlSanitizer, HtmlSanitizer>();
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
            builder.Services.AddScoped<IWarningFactory, WarningFactory>();
            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IMessageFactory, MessageFactory>();
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
            app.MapHub<ChatHub>("/chatHub");
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Newbie","VIP","Regular","Dinosaur" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            app.Use(async (context, next) =>
            {
                context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
                context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
                await next();
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
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
