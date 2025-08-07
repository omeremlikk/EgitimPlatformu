using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EgitimPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Features" },
                values: new object[] { "Lise Giriş Sınavı Matematik Hazırlık Paketi", "{\"matematik\":true}" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Features", "Grade" },
                values: new object[] { "Temel Yeterlilik Testi Matematik Hazırlık Paketi", "{\"matematik\":true}", "9-10. Sınıf" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Features" },
                values: new object[] { "Alan Yeterlilik Testi Matematik Hazırlık Paketi", "{\"matematik\":true}" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Features" },
                values: new object[] { "Lise Giriş Sınavı Hazırlık Paketi", "{\"matematik\":true,\"turkce\":true,\"fen\":true,\"sosyal\":true,\"ingilizce\":true}" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Features", "Grade" },
                values: new object[] { "Temel Yeterlilik Testi Hazırlık Paketi", "{\"matematik\":true,\"turkce\":true,\"fen\":true,\"sosyal\":true}", "11-12. Sınıf" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Features" },
                values: new object[] { "Alan Yeterlilik Testi Hazırlık Paketi", "{\"matematik\":true,\"fizik\":true,\"kimya\":true,\"biyoloji\":true}" });
        }
    }
}
