using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNamingOfLastSeenMEssages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.RenameColumn(
                name: "LastSeenByUser2Id",
                table: "ChatDataModels",
                newName: "LastMessageSeenByUser2Id");

            migrationBuilder.RenameColumn(
                name: "LastSeenByUser1Id",
                table: "ChatDataModels",
                newName: "LastMessageSeenByUser1Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatDataModels_LastSeenByUser2Id",
                table: "ChatDataModels",
                newName: "IX_ChatDataModels_LastMessageSeenByUser2Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatDataModels_LastSeenByUser1Id",
                table: "ChatDataModels",
                newName: "IX_ChatDataModels_LastMessageSeenByUser1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastMessageSeenByUser1Id",
                table: "ChatDataModels",
                column: "LastMessageSeenByUser1Id",
                principalTable: "MessageDataModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastMessageSeenByUser2Id",
                table: "ChatDataModels",
                column: "LastMessageSeenByUser2Id",
                principalTable: "MessageDataModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastMessageSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastMessageSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.RenameColumn(
                name: "LastMessageSeenByUser2Id",
                table: "ChatDataModels",
                newName: "LastSeenByUser2Id");

            migrationBuilder.RenameColumn(
                name: "LastMessageSeenByUser1Id",
                table: "ChatDataModels",
                newName: "LastSeenByUser1Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatDataModels_LastMessageSeenByUser2Id",
                table: "ChatDataModels",
                newName: "IX_ChatDataModels_LastSeenByUser2Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatDataModels_LastMessageSeenByUser1Id",
                table: "ChatDataModels",
                newName: "IX_ChatDataModels_LastSeenByUser1Id");

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
    }
}
