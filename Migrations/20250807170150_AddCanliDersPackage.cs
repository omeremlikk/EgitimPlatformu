using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EgitimPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class AddCanliDersPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "ColorCode", "CreatedAt", "Description", "DurationMonths", "Features", "Grade", "IconClass", "IsActive", "Name", "Price", "TestCount", "UpdatedAt", "VideoCount" },
                values: new object[] { 4, "#ff6b35", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Canlı Ders Paketi - Tüm Sınıflar İçin Matematik Dersleri", 6, "{\"matematik\":true,\"canli_ders\":true}", "Tüm Sınıflar", "fas fa-video", true, "Canlı Ders", 599.99m, 60, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
