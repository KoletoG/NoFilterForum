using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class NotificationDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationDataModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReplyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserFromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserToId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDataModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationDataModels_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationDataModels_AspNetUsers_UserToId",
                        column: x => x.UserToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationDataModels_ReplyDataModels_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "ReplyDataModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDataModels_ReplyId",
                table: "NotificationDataModels",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDataModels_UserFromId",
                table: "NotificationDataModels",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDataModels_UserToId",
                table: "NotificationDataModels",
                column: "UserToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationDataModels");
        }
    }
}
