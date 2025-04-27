using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class newreply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ReplyDataModels_UserId",
                table: "ReplyDataModels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReplyDataModels");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ReplyDataModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "ReplyDataModels");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReplyDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDataModels_UserId",
                table: "ReplyDataModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
