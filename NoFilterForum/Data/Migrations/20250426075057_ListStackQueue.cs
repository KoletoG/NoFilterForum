using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class ListStackQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModel_PostDataModel_ForPostId",
                table: "ReplyDataModel");

            migrationBuilder.DropIndex(
                name: "IX_ReplyDataModel_ForPostId",
                table: "ReplyDataModel");

            migrationBuilder.DropColumn(
                name: "ForPostId",
                table: "ReplyDataModel");

            migrationBuilder.AddColumn<string>(
                name: "PostDataModelId",
                table: "ReplyDataModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDataModel_PostDataModelId",
                table: "ReplyDataModel",
                column: "PostDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModel_PostDataModel_PostDataModelId",
                table: "ReplyDataModel",
                column: "PostDataModelId",
                principalTable: "PostDataModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModel_PostDataModel_PostDataModelId",
                table: "ReplyDataModel");

            migrationBuilder.DropIndex(
                name: "IX_ReplyDataModel_PostDataModelId",
                table: "ReplyDataModel");

            migrationBuilder.DropColumn(
                name: "PostDataModelId",
                table: "ReplyDataModel");

            migrationBuilder.AddColumn<string>(
                name: "ForPostId",
                table: "ReplyDataModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDataModel_ForPostId",
                table: "ReplyDataModel",
                column: "ForPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModel_PostDataModel_ForPostId",
                table: "ReplyDataModel",
                column: "ForPostId",
                principalTable: "PostDataModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
