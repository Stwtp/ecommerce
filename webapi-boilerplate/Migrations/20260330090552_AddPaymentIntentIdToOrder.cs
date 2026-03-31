using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi_boilerplate.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIntentIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Orders",
                type: "text",
                nullable: true);

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
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660), new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660), new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660), new DateTime(2026, 3, 30, 9, 5, 52, 745, DateTimeKind.Utc).AddTicks(1660) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Orders");

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
    }
}
