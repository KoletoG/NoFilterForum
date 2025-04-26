using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class TitleForPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PostDataModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "PostDataModel");
        }
    }
}
