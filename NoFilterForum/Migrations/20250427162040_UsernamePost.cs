using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class UsernamePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_AspNetUsers_UserId",
                table: "PostDataModels");

            migrationBuilder.DropIndex(
                name: "IX_PostDataModels_UserId",
                table: "PostDataModels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostDataModels");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "PostDataModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "PostDataModels");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostDataModels_UserId",
                table: "PostDataModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_AspNetUsers_UserId",
                table: "PostDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
