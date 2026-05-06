using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StargateApi.Business.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    LogLevel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    StackTrace = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AstronautDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentRank = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentDutyTitle = table.Column<string>(type: "TEXT", nullable: false),
                    CareerStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CareerEndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronautDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstronautDetails_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AstronautDuties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<string>(type: "TEXT", nullable: false),
                    DutyTitle = table.Column<string>(type: "TEXT", nullable: false),
                    DutyStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DutyEndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronautDuties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstronautDuties_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 13, "Jessica Meir" },
                    { 14, "Victor Glover" },
                    { 15, "Sunita Williams" },
                    { 16, "Michael Collins" },
                    { 17, "Eileen Collins" },
                    { 18, "Guion Bluford" },
                    { 19, "Anne McClain" },
                    { 20, "Jasmin Moghbeli" }
                });

            migrationBuilder.InsertData(
                table: "AstronautDetails",
                columns: new[] { "Id", "CareerEndDate", "CareerStartDate", "CurrentDutyTitle", "CurrentRank", "PersonId" },
                values: new object[,]
                {
                    { 2, null, new DateTime(2013, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Artemis Support Crew", "Mission Specialist", 13 },
                    { 3, null, new DateTime(2013, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Lunar Gateway Pilot", "Commander", 14 },
                    { 4, null, new DateTime(1998, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ISS Commander", "Captain", 15 },
                    { 5, new DateTime(1981, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1963, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), "RETIRED", "Major General", 16 },
                    { 6, new DateTime(2006, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "RETIRED", "Colonel", 17 },
                    { 7, new DateTime(1993, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1978, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "RETIRED", "Colonel", 18 },
                    { 8, null, new DateTime(2013, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Flight Engineer", "Lieutenant Colonel", 19 },
                    { 9, null, new DateTime(2017, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mission Commander", "Major", 20 }
                });

            migrationBuilder.InsertData(
                table: "AstronautDuties",
                columns: new[] { "Id", "DutyEndDate", "DutyStartDate", "DutyTitle", "PersonId", "Rank" },
                values: new object[,]
                {
                    { 2, null, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Artemis Support Crew", 13, "Mission Specialist" },
                    { 3, null, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lunar Gateway Pilot", 14, "Commander" },
                    { 4, null, new DateTime(2012, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), "ISS Commander", 15, "Captain" },
                    { 5, null, new DateTime(1981, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "RETIRED", 16, "Major General" },
                    { 6, null, new DateTime(2006, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), "RETIRED", 17, "Colonel" },
                    { 7, null, new DateTime(1993, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "RETIRED", 18, "Colonel" },
                    { 8, null, new DateTime(2021, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Flight Engineer", 19, "Lieutenant Colonel" },
                    { 9, null, new DateTime(2023, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Mission Commander", 20, "Major" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDetails_PersonId",
                table: "AstronautDetails",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDuties_PersonId",
                table: "AstronautDuties",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_People_Name",
                table: "People",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AstronautDetails");

            migrationBuilder.DropTable(
                name: "AstronautDuties");

            migrationBuilder.DropTable(
                name: "ProcessLog");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
