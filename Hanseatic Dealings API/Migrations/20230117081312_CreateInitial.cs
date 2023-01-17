using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HanseaticDealingsAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Xcord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ycord = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Money = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityStorageModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item = table.Column<int>(type: "int", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    Current = table.Column<int>(type: "int", nullable: false),
                    CityModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityStorageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityStorageModel_Cities_CityModelId",
                        column: x => x.CityModelId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShipStorageModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ShipModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipStorageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipStorageModel_Players_ShipModelId",
                        column: x => x.ShipModelId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityStorageModel_CityModelId",
                table: "CityStorageModel",
                column: "CityModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipStorageModel_ShipModelId",
                table: "ShipStorageModel",
                column: "ShipModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityStorageModel");

            migrationBuilder.DropTable(
                name: "ShipStorageModel");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
