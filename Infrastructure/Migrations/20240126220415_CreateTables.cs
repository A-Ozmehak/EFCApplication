using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Addresses",
                newName: "StreetName");

            migrationBuilder.AddColumn<string>(
                name: "StreetNumber",
                table: "Addresses",
                type: "nvarchar(10)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "StreetName",
                table: "Addresses",
                newName: "Address");
        }
    }
}
