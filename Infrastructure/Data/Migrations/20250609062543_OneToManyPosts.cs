using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFilterForum.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionId",
                table: "PostDataModels");

            migrationBuilder.AlterColumn<string>(
                name: "SectionId",
                table: "PostDataModels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionId",
                table: "PostDataModels",
                column: "SectionId",
                principalTable: "SectionDataModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionId",
                table: "PostDataModels");

            migrationBuilder.AlterColumn<string>(
                name: "SectionId",
                table: "PostDataModels",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDataModels_SectionDataModels_SectionId",
                table: "PostDataModels",
                column: "SectionId",
                principalTable: "SectionDataModels",
                principalColumn: "Id");
        }
    }
}
