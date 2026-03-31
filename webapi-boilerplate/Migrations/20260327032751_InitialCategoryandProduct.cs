using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi_boilerplate.Migrations
{
    /// <inheritdoc />
    public partial class InitialCategoryandProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7260), "Pokemon figures", true, "Pokemon", new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7260), "System" },
                    { 2, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270), "Digimon figures", true, "Digimon", new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270), "System" },
                    { 3, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270), "Mobile Suit Gundam plastic models", true, "Mobile Suit Gundam", new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270), "System" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "Stock", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), null, null, true, "Pikachu", 49.99m, 50, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), "System" },
                    { 2, 2, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), null, null, true, "Agumon", 29.99m, 100, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), "System" },
                    { 3, 3, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), null, null, true, "Gundam", 199.99m, 20, new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), "System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categories");
        }
    }
}
