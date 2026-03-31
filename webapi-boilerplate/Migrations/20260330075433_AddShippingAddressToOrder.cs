using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi_boilerplate.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingAddressToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4720), new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4720) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4720), new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4720) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4730), new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4730) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4810), new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4810) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4820), new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4820) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4820), new DateTime(2026, 3, 30, 7, 54, 33, 296, DateTimeKind.Utc).AddTicks(4820) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7260), new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7260) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270), new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270), new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340), new DateTime(2026, 3, 27, 3, 27, 51, 262, DateTimeKind.Utc).AddTicks(7340) });
        }
    }
}
