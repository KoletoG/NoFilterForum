using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModels_AspNetUsers_UserId",
                table: "ReplyDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
