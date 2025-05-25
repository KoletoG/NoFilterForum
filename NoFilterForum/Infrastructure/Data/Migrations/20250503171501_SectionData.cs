using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class SectionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SectionDataModelId",
                table: "PostDataModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SectionDataModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDataModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostDataModels_SectionDataModelId",
                table: "PostDataModels",
                column: "SectionDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionDataModelId",
                table: "PostDataModels",
                column: "SectionDataModelId",
                principalTable: "SectionDataModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionDataModelId",
                table: "PostDataModels");

            migrationBuilder.DropTable(
                name: "SectionDataModels");

            migrationBuilder.DropIndex(
                name: "IX_PostDataModels_SectionDataModelId",
                table: "PostDataModels");

            migrationBuilder.DropColumn(
                name: "SectionDataModelId",
                table: "PostDataModels");
        }
    }
}
