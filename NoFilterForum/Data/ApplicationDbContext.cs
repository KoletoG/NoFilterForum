using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Models;

namespace NoFilterForum.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserDataModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ReplyDataModel> ReplyDataModel { get; set; }
        public DbSet<PostDataModel> PostDataModel { get; set; }
    }
}
