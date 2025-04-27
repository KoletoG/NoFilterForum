using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class ReplyUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReplyDataModels",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ReplyDataModels_UserId",
                table: "ReplyDataModels");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReplyDataModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
