using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class WarningDataToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Warnings",
                table: "AspNetUsers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<byte>(
                name: "Warnings",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
