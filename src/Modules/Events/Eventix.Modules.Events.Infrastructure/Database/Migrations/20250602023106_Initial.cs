using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Modules.Events.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "events");

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "VARCHAR(80)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Street = table.Column<string>(type: "VARCHAR(200)", nullable: true),
                    Number = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Neighborhood = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ZipCode = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    City = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    State = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events",
                schema: "events");
        }
    }
}
