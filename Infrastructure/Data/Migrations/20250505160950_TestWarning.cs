using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class TestWarning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarningDataModels_AspNetUsers_UserDataModelId",
                table: "WarningDataModels");

            migrationBuilder.DropIndex(
                name: "IX_WarningDataModels_UserDataModelId",
                table: "WarningDataModels");

            migrationBuilder.DropColumn(
                name: "UserDataModelId",
                table: "WarningDataModels");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WarningDataModels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WarningDataModels_UserId",
                table: "WarningDataModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarningDataModels_AspNetUsers_UserId",
                table: "WarningDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarningDataModels_AspNetUsers_UserId",
                table: "WarningDataModels");

            migrationBuilder.DropIndex(
                name: "IX_WarningDataModels_UserId",
                table: "WarningDataModels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WarningDataModels");

            migrationBuilder.AddColumn<string>(
                name: "UserDataModelId",
                table: "WarningDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarningDataModels_UserDataModelId",
                table: "WarningDataModels",
                column: "UserDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarningDataModels_AspNetUsers_UserDataModelId",
                table: "WarningDataModels",
                column: "UserDataModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
