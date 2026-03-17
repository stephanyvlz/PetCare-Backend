using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationVeterinariansToClinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "id_clinic",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_id_clinic",
                table: "Users",
                column: "id_clinic");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Clinics_id_clinic",
                table: "Users",
                column: "id_clinic",
                principalTable: "Clinics",
                principalColumn: "id_clinic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Clinics_id_clinic",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_id_clinic",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "id_clinic",
                table: "Users");
        }
    }
}
