using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Discount.GRPC.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Rate = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Description", "ProductId", "Rate" },
                values: new object[,]
                {
                    { 1, "IPhone Discount", new Guid("019684e3-9bdd-7cf5-812c-3bc984d94b9e"), 0.29999999999999999 },
                    { 2, "IPhone Discount", new Guid("019684e3-9bdd-7cf5-812c-3bc984d94b9e"), 0.29999999999999999 },
                    { 3, "Samsung Discount", new Guid("019684e3-9bdd-747f-a9f6-59f022b24aae"), 0.10000000000000001 },
                    { 4, "Macbook Discount", new Guid("019684e3-9bdd-741e-b3a2-6869f6e0a414"), 0.10000000000000001 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
