using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_P22.Migrations
{
    /// <inheritdoc />
    public partial class Promotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                schema: "site",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPromotions",
                schema: "site",
                columns: table => new
                {
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPromotions", x => new { x.ProductsId, x.PromotionsId });
                    table.ForeignKey(
                        name: "FK_ProductPromotions_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalSchema: "site",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPromotions_Promotions_PromotionsId",
                        column: x => x.PromotionsId,
                        principalSchema: "site",
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "site",
                table: "Promotions",
                columns: new[] { "Id", "Description", "DiscountPercentage", "EndDate", "Name", "StartDate" },
                values: new object[] { new Guid("83f30e4c-4378-4496-91da-77bf4e8cfb88"), "Знижка на всі товари 10%", 10m, new DateTime(2025, 4, 29, 17, 22, 17, 963, DateTimeKind.Utc).AddTicks(7270), "Весняний розпродаж", new DateTime(2025, 3, 29, 17, 22, 17, 963, DateTimeKind.Utc).AddTicks(7267) });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPromotions_PromotionsId",
                schema: "site",
                table: "ProductPromotions",
                column: "PromotionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPromotions",
                schema: "site");

            migrationBuilder.DropTable(
                name: "Promotions",
                schema: "site");
        }
    }
}
