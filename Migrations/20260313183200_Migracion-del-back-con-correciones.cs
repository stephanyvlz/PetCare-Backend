using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetCare.API.Migrations
{
    /// <inheritdoc />
    public partial class Migraciondelbackconcorreciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tratamientos");

            migrationBuilder.DropTable(
                name: "consultas");

            migrationBuilder.DropTable(
                name: "citas");

            migrationBuilder.DropTable(
                name: "clinicas");

            migrationBuilder.DropTable(
                name: "mascotas");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    id_clinic = table.Column<Guid>(type: "uuid", nullable: false),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    schedule = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.id_clinic);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id_role = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id_role);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    id_role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id_user);
                    table.ForeignKey(
                        name: "FK_Users_Role_id_role",
                        column: x => x.id_role,
                        principalTable: "Role",
                        principalColumn: "id_role",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    id_pet = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    breed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    weight = table.Column<decimal>(type: "numeric", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: false),
                    id_user = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.id_pet);
                    table.ForeignKey(
                        name: "FK_Pets_Users_id_user",
                        column: x => x.id_user,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    id_appointment = table.Column<Guid>(type: "uuid", nullable: false),
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    id_pet = table.Column<Guid>(type: "uuid", nullable: false),
                    id_clinic = table.Column<Guid>(type: "uuid", nullable: false),
                    id_veterinarian = table.Column<Guid>(type: "uuid", nullable: false),
                    appointment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    service = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cost = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.id_appointment);
                    table.ForeignKey(
                        name: "FK_Appointments_Clinics_id_clinic",
                        column: x => x.id_clinic,
                        principalTable: "Clinics",
                        principalColumn: "id_clinic",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Pets_id_pet",
                        column: x => x.id_pet,
                        principalTable: "Pets",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_id_user",
                        column: x => x.id_user,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_id_veterinarian",
                        column: x => x.id_veterinarian,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    id_consultation = table.Column<Guid>(type: "uuid", nullable: false),
                    id_appointment = table.Column<Guid>(type: "uuid", nullable: false),
                    diagnosis = table.Column<string>(type: "text", nullable: false),
                    observation = table.Column<string>(type: "text", nullable: false),
                    consultation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.id_consultation);
                    table.ForeignKey(
                        name: "FK_Consultations_Appointments_id_appointment",
                        column: x => x.id_appointment,
                        principalTable: "Appointments",
                        principalColumn: "id_appointment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    treatment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id_consultation = table.Column<Guid>(type: "uuid", nullable: false),
                    medication = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    dosage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    duration = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.treatment_id);
                    table.ForeignKey(
                        name: "FK_Treatments_Consultations_id_consultation",
                        column: x => x.id_consultation,
                        principalTable: "Consultations",
                        principalColumn: "id_consultation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id_role", "role_name" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "veterinario" },
                    { 3, "cliente" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_id_clinic",
                table: "Appointments",
                column: "id_clinic");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_id_pet",
                table: "Appointments",
                column: "id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_id_user",
                table: "Appointments",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_id_veterinarian",
                table: "Appointments",
                column: "id_veterinarian");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_id_appointment",
                table: "Consultations",
                column: "id_appointment",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_id_user",
                table: "Pets",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_id_consultation",
                table: "Treatments",
                column: "id_consultation");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_id_role",
                table: "Users",
                column: "id_role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.CreateTable(
                name: "clinicas",
                columns: table => new
                {
                    IdClinica = table.Column<Guid>(type: "uuid", nullable: false),
                    Horario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinicas", x => x.IdClinica);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreRol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_RolId",
                        column: x => x.RolId,
                        principalTable: "roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mascotas",
                columns: table => new
                {
                    IdMascota = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    Edad = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Peso = table.Column<decimal>(type: "numeric", nullable: false),
                    Raza = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mascotas", x => x.IdMascota);
                    table.ForeignKey(
                        name: "FK_mascotas_usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    IdCita = table.Column<Guid>(type: "uuid", nullable: false),
                    IdClinica = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMascota = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdVeterinario = table.Column<Guid>(type: "uuid", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Servicio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citas", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_citas_clinicas_IdClinica",
                        column: x => x.IdClinica,
                        principalTable: "clinicas",
                        principalColumn: "IdClinica",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_citas_mascotas_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "mascotas",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_citas_usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_citas_usuarios_IdVeterinario",
                        column: x => x.IdVeterinario,
                        principalTable: "usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "consultas",
                columns: table => new
                {
                    IdConsulta = table.Column<Guid>(type: "uuid", nullable: false),
                    IdCita = table.Column<Guid>(type: "uuid", nullable: false),
                    Diagnostico = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultas", x => x.IdConsulta);
                    table.ForeignKey(
                        name: "FK_consultas_citas_IdCita",
                        column: x => x.IdCita,
                        principalTable: "citas",
                        principalColumn: "IdCita",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tratamientos",
                columns: table => new
                {
                    IdTratamiento = table.Column<Guid>(type: "uuid", nullable: false),
                    IdConsulta = table.Column<Guid>(type: "uuid", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric", nullable: false),
                    Dosis = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Medicamento = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Plazo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tratamientos", x => x.IdTratamiento);
                    table.ForeignKey(
                        name: "FK_tratamientos_consultas_IdConsulta",
                        column: x => x.IdConsulta,
                        principalTable: "consultas",
                        principalColumn: "IdConsulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "IdRol", "NombreRol" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "veterinario" },
                    { 3, "cliente" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_citas_IdClinica",
                table: "citas",
                column: "IdClinica");

            migrationBuilder.CreateIndex(
                name: "IX_citas_IdMascota",
                table: "citas",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_citas_IdUsuario",
                table: "citas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_citas_IdVeterinario",
                table: "citas",
                column: "IdVeterinario");

            migrationBuilder.CreateIndex(
                name: "IX_consultas_IdCita",
                table: "consultas",
                column: "IdCita",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mascotas_IdUsuario",
                table: "mascotas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_tratamientos_IdConsulta",
                table: "tratamientos",
                column: "IdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_Correo",
                table: "usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_RolId",
                table: "usuarios",
                column: "RolId");
        }
    }
}
