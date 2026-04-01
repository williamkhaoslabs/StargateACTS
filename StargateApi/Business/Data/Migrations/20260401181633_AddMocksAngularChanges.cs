using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateApi.Business.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMocksAngularChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDetail_Person_PersonId",
                table: "AstronautDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDuty_Person_PersonId",
                table: "AstronautDuty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AstronautDuty",
                table: "AstronautDuty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AstronautDetail",
                table: "AstronautDetail");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "People");

            migrationBuilder.RenameTable(
                name: "AstronautDuty",
                newName: "AstronautDuties");

            migrationBuilder.RenameTable(
                name: "AstronautDetail",
                newName: "AstronautDetails");

            migrationBuilder.RenameIndex(
                name: "IX_Person_Name",
                table: "People",
                newName: "IX_People_Name");

            migrationBuilder.RenameIndex(
                name: "IX_AstronautDuty_PersonId",
                table: "AstronautDuties",
                newName: "IX_AstronautDuties_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_AstronautDetail_PersonId",
                table: "AstronautDetails",
                newName: "IX_AstronautDetails_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AstronautDuties",
                table: "AstronautDuties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AstronautDetails",
                table: "AstronautDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDetails_People_PersonId",
                table: "AstronautDetails",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDuties_People_PersonId",
                table: "AstronautDuties",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDetails_People_PersonId",
                table: "AstronautDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDuties_People_PersonId",
                table: "AstronautDuties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AstronautDuties",
                table: "AstronautDuties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AstronautDetails",
                table: "AstronautDetails");

            migrationBuilder.RenameTable(
                name: "People",
                newName: "Person");

            migrationBuilder.RenameTable(
                name: "AstronautDuties",
                newName: "AstronautDuty");

            migrationBuilder.RenameTable(
                name: "AstronautDetails",
                newName: "AstronautDetail");

            migrationBuilder.RenameIndex(
                name: "IX_People_Name",
                table: "Person",
                newName: "IX_Person_Name");

            migrationBuilder.RenameIndex(
                name: "IX_AstronautDuties_PersonId",
                table: "AstronautDuty",
                newName: "IX_AstronautDuty_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_AstronautDetails_PersonId",
                table: "AstronautDetail",
                newName: "IX_AstronautDetail_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AstronautDuty",
                table: "AstronautDuty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AstronautDetail",
                table: "AstronautDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDetail_Person_PersonId",
                table: "AstronautDetail",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDuty_Person_PersonId",
                table: "AstronautDuty",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
