using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class UserIdForMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageDataModels_ChatDataModels_ChatDataModelId1",
                table: "MessageDataModels");

            migrationBuilder.DropIndex(
                name: "IX_MessageDataModels_ChatDataModelId1",
                table: "MessageDataModels");

            migrationBuilder.DropColumn(
                name: "ChatDataModelId1",
                table: "MessageDataModels");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MessageDataModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MessageDataModels");

            migrationBuilder.AddColumn<string>(
                name: "ChatDataModelId1",
                table: "MessageDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageDataModels_ChatDataModelId1",
                table: "MessageDataModels",
                column: "ChatDataModelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageDataModels_ChatDataModels_ChatDataModelId1",
                table: "MessageDataModels",
                column: "ChatDataModelId1",
                principalTable: "ChatDataModels",
                principalColumn: "Id");
        }
    }
}
