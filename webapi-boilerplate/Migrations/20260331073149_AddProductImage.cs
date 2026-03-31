using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi_boilerplate.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1740), new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1740) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1740), new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1740) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1740), new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1750) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1860), "https://dreamtoy.co.th/media/catalog/product/cache/d68a78be757bdca23b769dd800292e8d/7/0/7016276-1.jpg", new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1860) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1860), "https://inwfile.com/s-dc/8dp9yv.jpg", new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1870) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1870), "https://dr.lnwfile.com/_/dr/_raw/0m/qn/ea.jpg", new DateTime(2026, 3, 31, 7, 31, 49, 230, DateTimeKind.Utc).AddTicks(1870) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1600), new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1600) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1600), new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1600) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1600), new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1600) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660), null, new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660), null, new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660), null, new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660) });
        }
    }
}
