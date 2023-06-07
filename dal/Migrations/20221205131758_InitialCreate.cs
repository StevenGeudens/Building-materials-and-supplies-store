using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dal.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorieen",
                columns: table => new
                {
                    CategorieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorieen", x => x.CategorieId);
                });

            migrationBuilder.CreateTable(
                name: "Klanten",
                columns: table => new
                {
                    KlantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefoon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuisNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plaats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BtwNummer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klanten", x => x.KlantId);
                });

            migrationBuilder.CreateTable(
                name: "Vestigingen",
                columns: table => new
                {
                    VestigingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefoon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuisNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plaats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vestigingen", x => x.VestigingId);
                });

            migrationBuilder.CreateTable(
                name: "Artikels",
                columns: table => new
                {
                    ArtikelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prijs = table.Column<decimal>(type: "money", nullable: false),
                    EcoCheques = table.Column<bool>(type: "bit", nullable: false),
                    CategorieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikels", x => x.ArtikelId);
                    table.ForeignKey(
                        name: "FK_Artikels_Categorieen_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categorieen",
                        principalColumn: "CategorieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BtwPercentage = table.Column<int>(type: "int", nullable: false),
                    KlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Klanten_KlantId",
                        column: x => x.KlantId,
                        principalTable: "Klanten",
                        principalColumn: "KlantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aantal = table.Column<int>(type: "int", nullable: false),
                    VestigingId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_Stocks_Artikels_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikels",
                        principalColumn: "ArtikelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Vestigingen_VestigingId",
                        column: x => x.VestigingId,
                        principalTable: "Vestigingen",
                        principalColumn: "VestigingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WinkelmandItems",
                columns: table => new
                {
                    WinkelmandItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aantal = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinkelmandItems", x => x.WinkelmandItemId);
                    table.ForeignKey(
                        name: "FK_WinkelmandItems_Artikels_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikels",
                        principalColumn: "ArtikelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orderlijnen",
                columns: table => new
                {
                    OrderlijnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aantal = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orderlijnen", x => x.OrderlijnId);
                    table.ForeignKey(
                        name: "FK_Orderlijnen_Artikels_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikels",
                        principalColumn: "ArtikelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orderlijnen_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artikels_CategorieId",
                table: "Artikels",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_Orderlijnen_ArtikelId",
                table: "Orderlijnen",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_Orderlijnen_OrderId",
                table: "Orderlijnen",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KlantId",
                table: "Orders",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ArtikelId",
                table: "Stocks",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_VestigingId",
                table: "Stocks",
                column: "VestigingId");

            migrationBuilder.CreateIndex(
                name: "IX_WinkelmandItems_ArtikelId",
                table: "WinkelmandItems",
                column: "ArtikelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orderlijnen");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "WinkelmandItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Vestigingen");

            migrationBuilder.DropTable(
                name: "Artikels");

            migrationBuilder.DropTable(
                name: "Klanten");

            migrationBuilder.DropTable(
                name: "Categorieen");
        }
    }
}
