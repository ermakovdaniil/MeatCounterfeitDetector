using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.ResultDB
{
    public partial class ReturnedPaths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EncodedImage",
                table: "ResultImages",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "EncodedImage",
                table: "OriginalImages",
                newName: "ImagePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "ResultImages",
                newName: "EncodedImage");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "OriginalImages",
                newName: "EncodedImage");
        }
    }
}
