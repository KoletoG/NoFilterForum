using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class LastSeenByUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastSeenByUser1Id",
                table: "ChatDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastSeenByUser2Id",
                table: "ChatDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatDataModels_LastSeenByUser1Id",
                table: "ChatDataModels",
                column: "LastSeenByUser1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatDataModels_LastSeenByUser2Id",
                table: "ChatDataModels",
                column: "LastSeenByUser2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastSeenByUser1Id",
                table: "ChatDataModels",
                column: "LastSeenByUser1Id",
                principalTable: "MessageDataModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastSeenByUser2Id",
                table: "ChatDataModels",
                column: "LastSeenByUser2Id",
                principalTable: "MessageDataModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ChatDataModels_LastSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ChatDataModels_LastSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.DropColumn(
                name: "LastSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropColumn(
                name: "LastSeenByUser2Id",
                table: "ChatDataModels");
        }
    }
}
