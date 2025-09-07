using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NEWWWSTART : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastMessageSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatDataModels_MessageDataModels_LastMessageSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageDataModels_ChatDataModels_ChatDataModelId",
                table: "MessageDataModels");

            migrationBuilder.DropIndex(
                name: "IX_MessageDataModels_ChatDataModelId",
                table: "MessageDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ChatDataModels_LastMessageSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropIndex(
                name: "IX_ChatDataModels_LastMessageSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.DropColumn(
                name: "ChatDataModelId",
                table: "MessageDataModels");

            migrationBuilder.DropColumn(
                name: "LastMessageSeenByUser1Id",
                table: "ChatDataModels");

            migrationBuilder.DropColumn(
                name: "LastMessageSeenByUser2Id",
                table: "ChatDataModels");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "MessageDataModels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastMessageSeenByUser1",
                table: "ChatDataModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastMessageSeenByUser2",
                table: "ChatDataModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageDataModels_ChatId",
                table: "MessageDataModels",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageDataModels_ChatDataModels_ChatId",
                table: "MessageDataModels",
                column: "ChatId",
                principalTable: "ChatDataModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageDataModels_ChatDataModels_ChatId",
                table: "MessageDataModels");

            migrationBuilder.DropIndex(
                name: "IX_MessageDataModels_ChatId",
                table: "MessageDataModels");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "MessageDataModels");

            migrationBuilder.DropColumn(
                name: "LastMessageSeenByUser1",
                table: "ChatDataModels");

            migrationBuilder.DropColumn(
                name: "LastMessageSeenByUser2",
                table: "ChatDataModels");

            migrationBuilder.AddColumn<string>(
                name: "ChatDataModelId",
                table: "MessageDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastMessageSeenByUser1Id",
                table: "ChatDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastMessageSeenByUser2Id",
                table: "ChatDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageDataModels_ChatDataModelId",
                table: "MessageDataModels",
                column: "ChatDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatDataModels_LastMessageSeenByUser1Id",
                table: "ChatDataModels",
                column: "LastMessageSeenByUser1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatDataModels_LastMessageSeenByUser2Id",
                table: "ChatDataModels",
                column: "LastMessageSeenByUser2Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_MessageDataModels_ChatDataModels_ChatDataModelId",
                table: "MessageDataModels",
                column: "ChatDataModelId",
                principalTable: "ChatDataModels",
                principalColumn: "Id");
        }
    }
}
