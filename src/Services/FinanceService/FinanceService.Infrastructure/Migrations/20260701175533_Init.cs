using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteCurrencies",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavoriteCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "finance",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                schema: "finance",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCurrencies_CurrencyId",
                schema: "finance",
                table: "UserFavoriteCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCurrencies_UserId_CurrencyId",
                schema: "finance",
                table: "UserFavoriteCurrencies",
                columns: new[] { "UserId", "CurrencyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteCurrencies",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "finance");
        }
    }
}
