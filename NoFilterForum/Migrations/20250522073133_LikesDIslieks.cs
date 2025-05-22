using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class LikesDIslieks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReactionsPostRepliesIds",
                table: "AspNetUsers",
                newName: "LikesPostRepliesIds");

            migrationBuilder.AddColumn<string>(
                name: "DislikesPostRepliesIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikesPostRepliesIds",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LikesPostRepliesIds",
                table: "AspNetUsers",
                newName: "ReactionsPostRepliesIds");
        }
    }
}
