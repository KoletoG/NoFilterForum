using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class SectionInPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionDataModelId",
                table: "PostDataModels");

            migrationBuilder.RenameColumn(
                name: "SectionDataModelId",
                table: "PostDataModels",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_PostDataModels_SectionDataModelId",
                table: "PostDataModels",
                newName: "IX_PostDataModels_SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionId",
                table: "PostDataModels",
                column: "SectionId",
                principalTable: "SectionDataModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionId",
                table: "PostDataModels");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "PostDataModels",
                newName: "SectionDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_PostDataModels_SectionId",
                table: "PostDataModels",
                newName: "IX_PostDataModels_SectionDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionDataModelId",
                table: "PostDataModels",
                column: "SectionDataModelId",
                principalTable: "SectionDataModels",
                principalColumn: "Id");
        }
    }
}
