using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRepliesList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_PostDataModels_PostDataModelId",
                table: "ReplyDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ReplyDataModels_PostDataModelId",
                table: "ReplyDataModels");

            migrationBuilder.DropColumn(
                name: "PostDataModelId",
                table: "ReplyDataModels");

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "ReplyDataModels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDataModels_PostId",
                table: "ReplyDataModels",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModels_PostDataModels_PostId",
                table: "ReplyDataModels",
                column: "PostId",
                principalTable: "PostDataModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyDataModels_PostDataModels_PostId",
                table: "ReplyDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ReplyDataModels_PostId",
                table: "ReplyDataModels");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "ReplyDataModels");

            migrationBuilder.AddColumn<string>(
                name: "PostDataModelId",
                table: "ReplyDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDataModels_PostDataModelId",
                table: "ReplyDataModels",
                column: "PostDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyDataModels_PostDataModels_PostDataModelId",
                table: "ReplyDataModels",
                column: "PostDataModelId",
                principalTable: "PostDataModels",
                principalColumn: "Id");
        }
    }
}
