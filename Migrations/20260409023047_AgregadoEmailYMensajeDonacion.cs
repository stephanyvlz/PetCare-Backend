using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoEmailYMensajeDonacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "donor_email",
                table: "donations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "donations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "donor_email",
                table: "donations");

            migrationBuilder.DropColumn(
                name: "message",
                table: "donations");
        }
    }
}
