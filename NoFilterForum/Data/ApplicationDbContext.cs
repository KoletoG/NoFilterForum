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
        public DbSet<ReplyDataModel> ReplyDataModels { get; set; }
        public DbSet<PostDataModel> PostDataModels { get; set; }
        public DbSet<SectionDataModel> SectionDataModels { get; set; }
        public DbSet<ReportDataModel> ReportDataModels { get; set; }
        public DbSet<WarningDataModel> WarningDataModels { get; set; }
    }
}
