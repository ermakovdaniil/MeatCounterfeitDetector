using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.CounterfeitKB
{
    public partial class ChangedWordPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterfeitPaths");

            migrationBuilder.CreateTable(
                name: "CounterfeitImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CounterfeitId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncodedImage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterfeitImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterfeitImages_Counterfeits_CounterfeitId",
                        column: x => x.CounterfeitId,
                        principalTable: "Counterfeits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CounterfeitImages_CounterfeitId",
                table: "CounterfeitImages",
                column: "CounterfeitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterfeitImages");

            migrationBuilder.CreateTable(
                name: "CounterfeitPaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CounterfeitId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterfeitPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterfeitPaths_Counterfeits_CounterfeitId",
                        column: x => x.CounterfeitId,
                        principalTable: "Counterfeits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CounterfeitPaths_CounterfeitId",
                table: "CounterfeitPaths",
                column: "CounterfeitId");
        }
    }
}
