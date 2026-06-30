using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
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
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCurrencies_CurrencyId",
                table: "UserFavoriteCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCurrencies_UserId_CurrencyId",
                table: "UserFavoriteCurrencies",
                columns: new[] { "UserId", "CurrencyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteCurrencies");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
