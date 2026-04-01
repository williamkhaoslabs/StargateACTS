using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StargateApi.Business.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Person",
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
                table: "AstronautDetail",
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
                table: "AstronautDuty",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "John Doe" },
                    { 2, "Jane Doe" }
                });

            migrationBuilder.InsertData(
                table: "AstronautDetail",
                columns: new[] { "Id", "CareerEndDate", "CareerStartDate", "CurrentDutyTitle", "CurrentRank", "PersonId" },
                values: new object[] { 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Commander", "1LT", 1 });

            migrationBuilder.InsertData(
                table: "AstronautDuty",
                columns: new[] { "Id", "DutyEndDate", "DutyStartDate", "DutyTitle", "PersonId", "Rank" },
                values: new object[] { 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Commander", 1, "1LT" });
        }
    }
}
