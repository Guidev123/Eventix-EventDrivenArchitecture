using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Modules.Attendance.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "attendance");

            migrationBuilder.CreateTable(
                name: "Attendees",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(160)", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "attendance",
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
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventStatistics",
                schema: "attendance",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    TicketsSold = table.Column<int>(type: "int", nullable: false),
                    AttendeesCheckedIn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatistics", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessageConsumers",
                schema: "attendance",
                columns: table => new
                {
                    InboxMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessageConsumers", x => new { x.InboxMessageId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Content = table.Column<string>(type: "VARCHAR(3000)", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "VARCHAR(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessageConsumers",
                schema: "attendance",
                columns: table => new
                {
                    OutboxMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageConsumers", x => new { x.OutboxMessageId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Content = table.Column<string>(type: "VARCHAR(3000)", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "VARCHAR(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    UsedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Attendees_AttendeeId",
                        column: x => x.AttendeeId,
                        principalSchema: "attendance",
                        principalTable: "Attendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "attendance",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DuplicateCheckInTickets",
                schema: "attendance",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuplicateCheckInTickets", x => new { x.EventId, x.Code });
                    table.ForeignKey(
                        name: "FK_DuplicateCheckInTickets_EventStatistics_EventId",
                        column: x => x.EventId,
                        principalSchema: "attendance",
                        principalTable: "EventStatistics",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvalidCheckInTickets",
                schema: "attendance",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvalidCheckInTickets", x => new { x.EventId, x.Code });
                    table.ForeignKey(
                        name: "FK_InvalidCheckInTickets_EventStatistics_EventId",
                        column: x => x.EventId,
                        principalSchema: "attendance",
                        principalTable: "EventStatistics",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_Email",
                schema: "attendance",
                table: "Attendees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DuplicateCheckInTickets_Code",
                schema: "attendance",
                table: "DuplicateCheckInTickets",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_InvalidCheckInTickets_Code",
                schema: "attendance",
                table: "InvalidCheckInTickets",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AttendeeId",
                schema: "attendance",
                table: "Tickets",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Code",
                schema: "attendance",
                table: "Tickets",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                schema: "attendance",
                table: "Tickets",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuplicateCheckInTickets",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "InboxMessageConsumers",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "InvalidCheckInTickets",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "OutboxMessageConsumers",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "Tickets",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "EventStatistics",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "Attendees",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "attendance");
        }
    }
}
