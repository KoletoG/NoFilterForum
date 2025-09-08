using Core.Models.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserDataModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<WarningDataModel>().Property(x => x.IsAccepted).HasDefaultValue(false);
            builder.Entity<PostDataModel>().HasOne(x => x.Section).WithMany(x => x.Posts).HasForeignKey(x=>x.SectionId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserDataModel>().HasIndex(x => x.NormalizedUserName).IsUnique();
            builder.Entity<UserDataModel>().HasIndex(x => x.NormalizedEmail).IsUnique();
            builder.Entity<UserDataModel>().HasMany(x=>x.Posts).WithOne(x=>x.User).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<UserDataModel>().HasMany(x=>x.Replies).WithOne(x=>x.User).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<PostDataModel>().HasMany(x => x.Replies).WithOne(x => x.Post).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ChatDataModel>().HasMany(x => x.Messages).WithOne(x=>x.Chat).HasForeignKey(x=>x.ChatId).OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<ReplyDataModel> ReplyDataModels { get; set; }
        public DbSet<PostDataModel> PostDataModels { get; set; }
        public DbSet<SectionDataModel> SectionDataModels { get; set; }
        public DbSet<ReportDataModel> ReportDataModels { get; set; }
        public DbSet<WarningDataModel> WarningDataModels { get; set; }
        public DbSet<NotificationDataModel> NotificationDataModels { get; set; }
        public DbSet<ChatDataModel> ChatDataModels { get; set; }
        public DbSet<MessageDataModel> MessageDataModels { get; set; }
    }
}
