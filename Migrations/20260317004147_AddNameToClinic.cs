using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToClinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Clinics",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "Clinics");
        }
    }
}
