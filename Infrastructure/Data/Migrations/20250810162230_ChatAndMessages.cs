using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class ChatAndMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatDataModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatDataModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatDataModels_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatDataModels_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessageDataModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatDataModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChatDataModelId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageDataModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageDataModels_ChatDataModels_ChatDataModelId",
                        column: x => x.ChatDataModelId,
                        principalTable: "ChatDataModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageDataModels_ChatDataModels_ChatDataModelId1",
                        column: x => x.ChatDataModelId1,
                        principalTable: "ChatDataModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatDataModels_User1Id",
                table: "ChatDataModels",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatDataModels_User2Id",
                table: "ChatDataModels",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDataModels_ChatDataModelId",
                table: "MessageDataModels",
                column: "ChatDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDataModels_ChatDataModelId1",
                table: "MessageDataModels",
                column: "ChatDataModelId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageDataModels");

            migrationBuilder.DropTable(
                name: "ChatDataModels");
        }
    }
}
