using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.ResultDB
{
    public partial class ChangedWordPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_OriginalPaths_OrigPathId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_ResultPaths_ResPathId",
                table: "Results");

            migrationBuilder.DropTable(
                name: "ResultPaths");

            migrationBuilder.DropTable(
                name: "OriginalPaths");

            migrationBuilder.RenameColumn(
                name: "ResPathId",
                table: "Results",
                newName: "ResultImageId");

            migrationBuilder.RenameColumn(
                name: "OrigPathId",
                table: "Results",
                newName: "OriginalImageId");

            migrationBuilder.RenameColumn(
                name: "AnRes",
                table: "Results",
                newName: "AnalysisResult");

            migrationBuilder.RenameIndex(
                name: "IX_Results_ResPathId",
                table: "Results",
                newName: "IX_Results_ResultImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_OrigPathId",
                table: "Results",
                newName: "IX_Results_OriginalImageId");

            migrationBuilder.CreateTable(
                name: "OriginalImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EncodedImage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncodedImage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultImages_OriginalImages_OriginalImageId",
                        column: x => x.OriginalImageId,
                        principalTable: "OriginalImages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultImages_OriginalImageId",
                table: "ResultImages",
                column: "OriginalImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_OriginalImages_OriginalImageId",
                table: "Results",
                column: "OriginalImageId",
                principalTable: "OriginalImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_ResultImages_ResultImageId",
                table: "Results",
                column: "ResultImageId",
                principalTable: "ResultImages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_OriginalImages_OriginalImageId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_ResultImages_ResultImageId",
                table: "Results");

            migrationBuilder.DropTable(
                name: "ResultImages");

            migrationBuilder.DropTable(
                name: "OriginalImages");

            migrationBuilder.RenameColumn(
                name: "ResultImageId",
                table: "Results",
                newName: "ResPathId");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "Results",
                newName: "OrigPathId");

            migrationBuilder.RenameColumn(
                name: "AnalysisResult",
                table: "Results",
                newName: "AnRes");

            migrationBuilder.RenameIndex(
                name: "IX_Results_ResultImageId",
                table: "Results",
                newName: "IX_Results_ResPathId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_OriginalImageId",
                table: "Results",
                newName: "IX_Results_OrigPathId");

            migrationBuilder.CreateTable(
                name: "OriginalPaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalPaths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultPaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InitId = table.Column<Guid>(type: "uuid", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultPaths_OriginalPaths_InitId",
                        column: x => x.InitId,
                        principalTable: "OriginalPaths",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultPaths_InitId",
                table: "ResultPaths",
                column: "InitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_OriginalPaths_OrigPathId",
                table: "Results",
                column: "OrigPathId",
                principalTable: "OriginalPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_ResultPaths_ResPathId",
                table: "Results",
                column: "ResPathId",
                principalTable: "ResultPaths",
                principalColumn: "Id");
        }
    }
}
