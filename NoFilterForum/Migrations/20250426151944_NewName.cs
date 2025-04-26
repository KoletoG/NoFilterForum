using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class NewName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModel_AspNetUsers_UserId",
                table: "PostDataModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModel_PostDataModel_PostDataModelId",
                table: "ReplyDataModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReplyDataModel",
                table: "ReplyDataModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostDataModel",
                table: "PostDataModel");

            migrationBuilder.RenameTable(
                name: "ReplyDataModel",
                newName: "ReplyDataModels");

            migrationBuilder.RenameTable(
                name: "PostDataModel",
                newName: "PostDataModels");

            migrationBuilder.RenameIndex(
                name: "IX_ReplyDataModel_PostDataModelId",
                table: "ReplyDataModels",
                newName: "IX_ReplyDataModels_PostDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_PostDataModel_UserId",
                table: "PostDataModels",
                newName: "IX_PostDataModels_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReplyDataModels",
                table: "ReplyDataModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostDataModels",
                table: "PostDataModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_AspNetUsers_UserId",
                table: "PostDataModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModels_PostDataModels_PostDataModelId",
                table: "ReplyDataModels",
                column: "PostDataModelId",
                principalTable: "PostDataModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_AspNetUsers_UserId",
                table: "PostDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_PostDataModels_PostDataModelId",
                table: "ReplyDataModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReplyDataModels",
                table: "ReplyDataModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostDataModels",
                table: "PostDataModels");

            migrationBuilder.RenameTable(
                name: "ReplyDataModels",
                newName: "ReplyDataModel");

            migrationBuilder.RenameTable(
                name: "PostDataModels",
                newName: "PostDataModel");

            migrationBuilder.RenameIndex(
                name: "IX_ReplyDataModels_PostDataModelId",
                table: "ReplyDataModel",
                newName: "IX_ReplyDataModel_PostDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_PostDataModels_UserId",
                table: "PostDataModel",
                newName: "IX_PostDataModel_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReplyDataModel",
                table: "ReplyDataModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostDataModel",
                table: "PostDataModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModel_AspNetUsers_UserId",
                table: "PostDataModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModel_PostDataModel_PostDataModelId",
                table: "ReplyDataModel",
                column: "PostDataModelId",
                principalTable: "PostDataModel",
                principalColumn: "Id");
        }
    }
}
