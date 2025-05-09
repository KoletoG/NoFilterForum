using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class ChanbgedRepot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDataModels_AspNetUsers_UserId",
                table: "ReportDataModels");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReportDataModels",
                newName: "UserToId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportDataModels_UserId",
                table: "ReportDataModels",
                newName: "IX_ReportDataModels_UserToId");

            migrationBuilder.AddColumn<string>(
                name: "UserFromId",
                table: "ReportDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportDataModels_UserFromId",
                table: "ReportDataModels",
                column: "UserFromId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDataModels_AspNetUsers_UserFromId",
                table: "ReportDataModels",
                column: "UserFromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDataModels_AspNetUsers_UserToId",
                table: "ReportDataModels",
                column: "UserToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDataModels_AspNetUsers_UserFromId",
                table: "ReportDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportDataModels_AspNetUsers_UserToId",
                table: "ReportDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ReportDataModels_UserFromId",
                table: "ReportDataModels");

            migrationBuilder.DropColumn(
                name: "UserFromId",
                table: "ReportDataModels");

            migrationBuilder.RenameColumn(
                name: "UserToId",
                table: "ReportDataModels",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportDataModels_UserToId",
                table: "ReportDataModels",
                newName: "IX_ReportDataModels_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDataModels_AspNetUsers_UserId",
                table: "ReportDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
