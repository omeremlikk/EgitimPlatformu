using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EgitimPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Grade = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VideoCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TestCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMonths = table.Column<int>(type: "INTEGER", nullable: false),
                    Features = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ColorCode = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    IconClass = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, defaultValue: "Student"),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackageRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PackageId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResponseMessage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageRequests_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PackageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPackages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPackages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "ColorCode", "CreatedAt", "Description", "DurationMonths", "Features", "Grade", "IconClass", "IsActive", "Name", "Price", "TestCount", "UpdatedAt", "VideoCount" },
                values: new object[,]
                {
                    { 1, "#28a745", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lise Giriş Sınavı Hazırlık Paketi", 12, "{\"matematik\":true,\"turkce\":true,\"fen\":true,\"sosyal\":true,\"ingilizce\":true}", "8. Sınıf", "fas fa-school", true, "LGS", 299.99m, 25, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45 },
                    { 2, "#007bff", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Temel Yeterlilik Testi Hazırlık Paketi", 10, "{\"matematik\":true,\"turkce\":true,\"fen\":true,\"sosyal\":true}", "11-12. Sınıf", "fas fa-university", true, "TYT", 399.99m, 40, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60 },
                    { 3, "#dc3545", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alan Yeterlilik Testi Hazırlık Paketi", 8, "{\"matematik\":true,\"fizik\":true,\"kimya\":true,\"biyoloji\":true}", "12. Sınıf", "fas fa-medal", false, "AYT", 499.99m, 50, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@egitim.com", "Gurbet", true, "CATIN", "3mI8K90ZhwKbSNBnvD9sAJcOz1EwQe7so6W2Ua3FhgM=", "5551234567", "Teacher" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ogrenci@egitim.com", "Ahmet", true, "YILMAZ", "xJZiO3YTlMr+mKI2AFkl39cmcxL7HRVrQHlDW5p6v1g=", "5559876543", "Student" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageRequests_PackageId",
                table: "PackageRequests",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageRequests_UserId_PackageId",
                table: "PackageRequests",
                columns: new[] { "UserId", "PackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Name",
                table: "Packages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPackages_PackageId",
                table: "UserPackages",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackages_UserId_PackageId",
                table: "UserPackages",
                columns: new[] { "UserId", "PackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PackageRequests");

            migrationBuilder.DropTable(
                name: "UserPackages");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
