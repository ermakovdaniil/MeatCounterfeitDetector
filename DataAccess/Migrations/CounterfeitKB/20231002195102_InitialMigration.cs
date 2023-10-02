#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.CounterfeitKB
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Counterfeits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counterfeits", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterfeitPaths");

            migrationBuilder.DropTable(
                name: "Counterfeits");
        }
    }
}
