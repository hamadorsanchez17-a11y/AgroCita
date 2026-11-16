using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgroCita.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fase2ModeloCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequestCrops_AssistanceRequests_SolicitudId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequestCrops_Crops_CultivoId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequests_Farms_FincaId",
                table: "AssistanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequests_Producers_ProductorId",
                table: "AssistanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_Farms_FincaId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_Producers_ProductorId",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_AssistanceRequests_SolicitudId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Farms_FincaId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Technicians_TecnicoId",
                table: "Visits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visits",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_FincaId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_TecnicoId",
                table: "Visits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Producers",
                table: "Producers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Farms",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ProductorId",
                table: "Farms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Crops",
                table: "Crops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistanceRequests",
                table: "AssistanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequests_ProductorId",
                table: "AssistanceRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistanceRequestCrops",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "FincaId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "HoraProgramada",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "ProductorId",
                table: "AssistanceRequests");

            migrationBuilder.RenameTable(
                name: "Visits",
                newName: "Visita");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "Usuario");

            migrationBuilder.RenameTable(
                name: "Technicians",
                newName: "Tecnico");

            migrationBuilder.RenameTable(
                name: "Producers",
                newName: "Productor");

            migrationBuilder.RenameTable(
                name: "Farms",
                newName: "Finca");

            migrationBuilder.RenameTable(
                name: "Crops",
                newName: "Cultivo");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Empresa");

            migrationBuilder.RenameTable(
                name: "AssistanceRequests",
                newName: "Solicitud");

            migrationBuilder.RenameTable(
                name: "AssistanceRequestCrops",
                newName: "SolicitudCultivo");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Visita",
                newName: "FechaCreacion");

            migrationBuilder.RenameIndex(
                name: "IX_Visits_SolicitudId",
                table: "Visita",
                newName: "IX_Visita_SolicitudId");

            migrationBuilder.RenameIndex(
                name: "IX_usuarios_Email",
                table: "Usuario",
                newName: "IX_Usuario_Email");

            migrationBuilder.RenameColumn(
                name: "Dni",
                table: "Productor",
                newName: "DNI");

            migrationBuilder.RenameIndex(
                name: "IX_Crops_FincaId",
                table: "Cultivo",
                newName: "IX_Cultivo_FincaId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Solicitud",
                newName: "FechaCreacion");

            migrationBuilder.RenameIndex(
                name: "IX_AssistanceRequests_FincaId",
                table: "Solicitud",
                newName: "IX_Solicitud_FincaId");

            migrationBuilder.RenameIndex(
                name: "IX_AssistanceRequestCrops_CultivoId",
                table: "SolicitudCultivo",
                newName: "IX_SolicitudCultivo_CultivoId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaProgramada",
                table: "Visita",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Visita",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelacion",
                table: "Visita",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuario",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuario",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Usuario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Usuario",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Usuario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Tecnico",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Tecnico",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Productor",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Productor",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Productor",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Finca",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Finca",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Municipio",
                table: "Finca",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Finca",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Departamento",
                table: "Finca",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "AreaManzanas",
                table: "Finca",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Finca",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Cultivo",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Cultivo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "AreaSembrada",
                table: "Cultivo",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Cultivo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaSiembra",
                table: "Cultivo",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoCultivoId",
                table: "Cultivo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Variedad",
                table: "Cultivo",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoInvitacion",
                table: "Empresa",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Empresa",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Empresa",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Empresa",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Empresa",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RTN",
                table: "Empresa",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Empresa",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "Solicitud",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Solicitud",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ComentarioEstado",
                table: "Solicitud",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "SolicitudCultivo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visita",
                table: "Visita",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tecnico",
                table: "Tecnico",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productor",
                table: "Productor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Finca",
                table: "Finca",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cultivo",
                table: "Cultivo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Empresa",
                table: "Empresa",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solicitud",
                table: "Solicitud",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolicitudCultivo",
                table: "SolicitudCultivo",
                columns: new[] { "SolicitudId", "CultivoId" });

            migrationBuilder.CreateTable(
                name: "CategoriaCalidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaCalidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriaCalidad_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fertilizante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Composicion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fertilizante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fertilizante_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Informe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    Diagnostico = table.Column<string>(type: "text", nullable: false),
                    Recomendaciones = table.Column<string>(type: "text", nullable: false),
                    FechaElaboracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Informe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Informe_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Informe_Visita_VisitaId",
                        column: x => x.VisitaId,
                        principalTable: "Visita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PrecioMensual = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MaxTecnicos = table.Column<int>(type: "integer", nullable: true),
                    MaxProductores = table.Column<int>(type: "integer", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReprogramacionVisita",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    FechaAnterior = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaNueva = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReprogramacionVisita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReprogramacionVisita_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReprogramacionVisita_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReprogramacionVisita_Visita_VisitaId",
                        column: x => x.VisitaId,
                        principalTable: "Visita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCultivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCultivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoCultivo_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspeccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaCalidadId = table.Column<int>(type: "integer", nullable: true),
                    FechaInspeccion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspeccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspeccion_CategoriaCalidad_CategoriaCalidadId",
                        column: x => x.CategoriaCalidadId,
                        principalTable: "CategoriaCalidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspeccion_Cultivo_CultivoId",
                        column: x => x.CultivoId,
                        principalTable: "Cultivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspeccion_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspeccion_Visita_VisitaId",
                        column: x => x.VisitaId,
                        principalTable: "Visita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecomendacionFertilizacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    FertilizanteId = table.Column<int>(type: "integer", nullable: false),
                    DosisRecomendada = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Frecuencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Indicaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaRecomendacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecomendacionFertilizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecomendacionFertilizacion_Cultivo_CultivoId",
                        column: x => x.CultivoId,
                        principalTable: "Cultivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecomendacionFertilizacion_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecomendacionFertilizacion_Fertilizante_FertilizanteId",
                        column: x => x.FertilizanteId,
                        principalTable: "Fertilizante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecomendacionFertilizacion_Visita_VisitaId",
                        column: x => x.VisitaId,
                        principalTable: "Visita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suscripcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suscripcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suscripcion_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suscripcion_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParametroEvaluacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    TipoCultivoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Unidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ValorMinimo = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    ValorMaximo = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametroEvaluacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParametroEvaluacion_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParametroEvaluacion_TipoCultivo_TipoCultivoId",
                        column: x => x.TipoCultivoId,
                        principalTable: "TipoCultivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AplicacionFertilizante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    FertilizanteId = table.Column<int>(type: "integer", nullable: false),
                    RecomendacionId = table.Column<int>(type: "integer", nullable: true),
                    TecnicoId = table.Column<int>(type: "integer", nullable: false),
                    CantidadAplicada = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    FechaAplicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AplicacionFertilizante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AplicacionFertilizante_Cultivo_CultivoId",
                        column: x => x.CultivoId,
                        principalTable: "Cultivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AplicacionFertilizante_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AplicacionFertilizante_Fertilizante_FertilizanteId",
                        column: x => x.FertilizanteId,
                        principalTable: "Fertilizante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AplicacionFertilizante_RecomendacionFertilizacion_Recomenda~",
                        column: x => x.RecomendacionId,
                        principalTable: "RecomendacionFertilizacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AplicacionFertilizante_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoParametro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    InspeccionId = table.Column<int>(type: "integer", nullable: false),
                    ParametroEvaluacionId = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoParametro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoParametro_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadoParametro_Inspeccion_InspeccionId",
                        column: x => x.InspeccionId,
                        principalTable: "Inspeccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadoParametro_ParametroEvaluacion_ParametroEvaluacionId",
                        column: x => x.ParametroEvaluacionId,
                        principalTable: "ParametroEvaluacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "SuperAdmin" },
                    { 2, "AdminEmpresa" },
                    { 3, "Tecnico" },
                    { 4, "Productor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visita_EmpresaId_Estado",
                table: "Visita",
                columns: new[] { "EmpresaId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Visita_TecnicoId_FechaProgramada",
                table: "Visita",
                columns: new[] { "TecnicoId", "FechaProgramada" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EmpresaId",
                table: "Usuario",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Tecnico_EmpresaId",
                table: "Tecnico",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tecnico_UsuarioId",
                table: "Tecnico",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productor_EmpresaId_DNI",
                table: "Productor",
                columns: new[] { "EmpresaId", "DNI" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productor_UsuarioId",
                table: "Productor",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Finca_EmpresaId",
                table: "Finca",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Finca_ProductorId_Nombre",
                table: "Finca",
                columns: new[] { "ProductorId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cultivo_EmpresaId",
                table: "Cultivo",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultivo_TipoCultivoId",
                table: "Cultivo",
                column: "TipoCultivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_CodigoInvitacion",
                table: "Empresa",
                column: "CodigoInvitacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_RTN",
                table: "Empresa",
                column: "RTN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_EmpresaId_Estado",
                table: "Solicitud",
                columns: new[] { "EmpresaId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCultivo_EmpresaId",
                table: "SolicitudCultivo",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionFertilizante_CultivoId_FechaAplicacion",
                table: "AplicacionFertilizante",
                columns: new[] { "CultivoId", "FechaAplicacion" });

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionFertilizante_EmpresaId",
                table: "AplicacionFertilizante",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionFertilizante_FertilizanteId",
                table: "AplicacionFertilizante",
                column: "FertilizanteId");

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionFertilizante_RecomendacionId",
                table: "AplicacionFertilizante",
                column: "RecomendacionId");

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionFertilizante_TecnicoId",
                table: "AplicacionFertilizante",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaCalidad_EmpresaId_Nombre",
                table: "CategoriaCalidad",
                columns: new[] { "EmpresaId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fertilizante_EmpresaId_Nombre",
                table: "Fertilizante",
                columns: new[] { "EmpresaId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Informe_EmpresaId",
                table: "Informe",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Informe_VisitaId",
                table: "Informe",
                column: "VisitaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspeccion_CategoriaCalidadId",
                table: "Inspeccion",
                column: "CategoriaCalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspeccion_CultivoId_FechaInspeccion",
                table: "Inspeccion",
                columns: new[] { "CultivoId", "FechaInspeccion" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspeccion_EmpresaId",
                table: "Inspeccion",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspeccion_VisitaId_CultivoId",
                table: "Inspeccion",
                columns: new[] { "VisitaId", "CultivoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParametroEvaluacion_EmpresaId",
                table: "ParametroEvaluacion",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ParametroEvaluacion_TipoCultivoId_Nombre",
                table: "ParametroEvaluacion",
                columns: new[] { "TipoCultivoId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plan_Nombre",
                table: "Plan",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecomendacionFertilizacion_CultivoId",
                table: "RecomendacionFertilizacion",
                column: "CultivoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendacionFertilizacion_EmpresaId",
                table: "RecomendacionFertilizacion",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendacionFertilizacion_FertilizanteId",
                table: "RecomendacionFertilizacion",
                column: "FertilizanteId");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendacionFertilizacion_VisitaId",
                table: "RecomendacionFertilizacion",
                column: "VisitaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReprogramacionVisita_EmpresaId",
                table: "ReprogramacionVisita",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReprogramacionVisita_UsuarioId",
                table: "ReprogramacionVisita",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReprogramacionVisita_VisitaId",
                table: "ReprogramacionVisita",
                column: "VisitaId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoParametro_EmpresaId",
                table: "ResultadoParametro",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoParametro_InspeccionId_ParametroEvaluacionId",
                table: "ResultadoParametro",
                columns: new[] { "InspeccionId", "ParametroEvaluacionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoParametro_ParametroEvaluacionId",
                table: "ResultadoParametro",
                column: "ParametroEvaluacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suscripcion_EmpresaId_Estado",
                table: "Suscripcion",
                columns: new[] { "EmpresaId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Suscripcion_PlanId",
                table: "Suscripcion",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCultivo_EmpresaId_Nombre",
                table: "TipoCultivo",
                columns: new[] { "EmpresaId", "Nombre" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cultivo_Empresa_EmpresaId",
                table: "Cultivo",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cultivo_Finca_FincaId",
                table: "Cultivo",
                column: "FincaId",
                principalTable: "Finca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cultivo_TipoCultivo_TipoCultivoId",
                table: "Cultivo",
                column: "TipoCultivoId",
                principalTable: "TipoCultivo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Finca_Empresa_EmpresaId",
                table: "Finca",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Finca_Productor_ProductorId",
                table: "Finca",
                column: "ProductorId",
                principalTable: "Productor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productor_Empresa_EmpresaId",
                table: "Productor",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productor_Usuario_UsuarioId",
                table: "Productor",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitud_Empresa_EmpresaId",
                table: "Solicitud",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitud_Finca_FincaId",
                table: "Solicitud",
                column: "FincaId",
                principalTable: "Finca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCultivo_Cultivo_CultivoId",
                table: "SolicitudCultivo",
                column: "CultivoId",
                principalTable: "Cultivo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCultivo_Empresa_EmpresaId",
                table: "SolicitudCultivo",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCultivo_Solicitud_SolicitudId",
                table: "SolicitudCultivo",
                column: "SolicitudId",
                principalTable: "Solicitud",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tecnico_Empresa_EmpresaId",
                table: "Tecnico",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tecnico_Usuario_UsuarioId",
                table: "Tecnico",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Empresa_EmpresaId",
                table: "Usuario",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Rol_RolId",
                table: "Usuario",
                column: "RolId",
                principalTable: "Rol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visita_Empresa_EmpresaId",
                table: "Visita",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visita_Solicitud_SolicitudId",
                table: "Visita",
                column: "SolicitudId",
                principalTable: "Solicitud",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visita_Tecnico_TecnicoId",
                table: "Visita",
                column: "TecnicoId",
                principalTable: "Tecnico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cultivo_Empresa_EmpresaId",
                table: "Cultivo");

            migrationBuilder.DropForeignKey(
                name: "FK_Cultivo_Finca_FincaId",
                table: "Cultivo");

            migrationBuilder.DropForeignKey(
                name: "FK_Cultivo_TipoCultivo_TipoCultivoId",
                table: "Cultivo");

            migrationBuilder.DropForeignKey(
                name: "FK_Finca_Empresa_EmpresaId",
                table: "Finca");

            migrationBuilder.DropForeignKey(
                name: "FK_Finca_Productor_ProductorId",
                table: "Finca");

            migrationBuilder.DropForeignKey(
                name: "FK_Productor_Empresa_EmpresaId",
                table: "Productor");

            migrationBuilder.DropForeignKey(
                name: "FK_Productor_Usuario_UsuarioId",
                table: "Productor");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitud_Empresa_EmpresaId",
                table: "Solicitud");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitud_Finca_FincaId",
                table: "Solicitud");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudCultivo_Cultivo_CultivoId",
                table: "SolicitudCultivo");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudCultivo_Empresa_EmpresaId",
                table: "SolicitudCultivo");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudCultivo_Solicitud_SolicitudId",
                table: "SolicitudCultivo");

            migrationBuilder.DropForeignKey(
                name: "FK_Tecnico_Empresa_EmpresaId",
                table: "Tecnico");

            migrationBuilder.DropForeignKey(
                name: "FK_Tecnico_Usuario_UsuarioId",
                table: "Tecnico");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empresa_EmpresaId",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Rol_RolId",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Visita_Empresa_EmpresaId",
                table: "Visita");

            migrationBuilder.DropForeignKey(
                name: "FK_Visita_Solicitud_SolicitudId",
                table: "Visita");

            migrationBuilder.DropForeignKey(
                name: "FK_Visita_Tecnico_TecnicoId",
                table: "Visita");

            migrationBuilder.DropTable(
                name: "AplicacionFertilizante");

            migrationBuilder.DropTable(
                name: "Informe");

            migrationBuilder.DropTable(
                name: "ReprogramacionVisita");

            migrationBuilder.DropTable(
                name: "ResultadoParametro");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Suscripcion");

            migrationBuilder.DropTable(
                name: "RecomendacionFertilizacion");

            migrationBuilder.DropTable(
                name: "Inspeccion");

            migrationBuilder.DropTable(
                name: "ParametroEvaluacion");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "Fertilizante");

            migrationBuilder.DropTable(
                name: "CategoriaCalidad");

            migrationBuilder.DropTable(
                name: "TipoCultivo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visita",
                table: "Visita");

            migrationBuilder.DropIndex(
                name: "IX_Visita_EmpresaId_Estado",
                table: "Visita");

            migrationBuilder.DropIndex(
                name: "IX_Visita_TecnicoId_FechaProgramada",
                table: "Visita");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_EmpresaId",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tecnico",
                table: "Tecnico");

            migrationBuilder.DropIndex(
                name: "IX_Tecnico_EmpresaId",
                table: "Tecnico");

            migrationBuilder.DropIndex(
                name: "IX_Tecnico_UsuarioId",
                table: "Tecnico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolicitudCultivo",
                table: "SolicitudCultivo");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudCultivo_EmpresaId",
                table: "SolicitudCultivo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solicitud",
                table: "Solicitud");

            migrationBuilder.DropIndex(
                name: "IX_Solicitud_EmpresaId_Estado",
                table: "Solicitud");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productor",
                table: "Productor");

            migrationBuilder.DropIndex(
                name: "IX_Productor_EmpresaId_DNI",
                table: "Productor");

            migrationBuilder.DropIndex(
                name: "IX_Productor_UsuarioId",
                table: "Productor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Finca",
                table: "Finca");

            migrationBuilder.DropIndex(
                name: "IX_Finca_EmpresaId",
                table: "Finca");

            migrationBuilder.DropIndex(
                name: "IX_Finca_ProductorId_Nombre",
                table: "Finca");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Empresa",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_CodigoInvitacion",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_RTN",
                table: "Empresa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cultivo",
                table: "Cultivo");

            migrationBuilder.DropIndex(
                name: "IX_Cultivo_EmpresaId",
                table: "Cultivo");

            migrationBuilder.DropIndex(
                name: "IX_Cultivo_TipoCultivoId",
                table: "Cultivo");

            migrationBuilder.DropColumn(
                name: "MotivoCancelacion",
                table: "Visita");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "SolicitudCultivo");

            migrationBuilder.DropColumn(
                name: "ComentarioEstado",
                table: "Solicitud");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Finca");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "RTN",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Cultivo");

            migrationBuilder.DropColumn(
                name: "AreaSembrada",
                table: "Cultivo");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Cultivo");

            migrationBuilder.DropColumn(
                name: "FechaSiembra",
                table: "Cultivo");

            migrationBuilder.DropColumn(
                name: "TipoCultivoId",
                table: "Cultivo");

            migrationBuilder.DropColumn(
                name: "Variedad",
                table: "Cultivo");

            migrationBuilder.RenameTable(
                name: "Visita",
                newName: "Visits");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "usuarios");

            migrationBuilder.RenameTable(
                name: "Tecnico",
                newName: "Technicians");

            migrationBuilder.RenameTable(
                name: "SolicitudCultivo",
                newName: "AssistanceRequestCrops");

            migrationBuilder.RenameTable(
                name: "Solicitud",
                newName: "AssistanceRequests");

            migrationBuilder.RenameTable(
                name: "Productor",
                newName: "Producers");

            migrationBuilder.RenameTable(
                name: "Finca",
                newName: "Farms");

            migrationBuilder.RenameTable(
                name: "Empresa",
                newName: "Companies");

            migrationBuilder.RenameTable(
                name: "Cultivo",
                newName: "Crops");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Visits",
                newName: "UpdatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Visita_SolicitudId",
                table: "Visits",
                newName: "IX_Visits_SolicitudId");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_Email",
                table: "usuarios",
                newName: "IX_usuarios_Email");

            migrationBuilder.RenameIndex(
                name: "IX_SolicitudCultivo_CultivoId",
                table: "AssistanceRequestCrops",
                newName: "IX_AssistanceRequestCrops_CultivoId");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "AssistanceRequests",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitud_FincaId",
                table: "AssistanceRequests",
                newName: "IX_AssistanceRequests_FincaId");

            migrationBuilder.RenameColumn(
                name: "DNI",
                table: "Producers",
                newName: "Dni");

            migrationBuilder.RenameIndex(
                name: "IX_Cultivo_FincaId",
                table: "Crops",
                newName: "IX_Crops_FincaId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaProgramada",
                table: "Visits",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Visits",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Visits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FincaId",
                table: "Visits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraProgramada",
                table: "Visits",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "usuarios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Technicians",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Technicians",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "AssistanceRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "AssistanceRequests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "ProductorId",
                table: "AssistanceRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Producers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Producers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Producers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Producers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "Producers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Farms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Farms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Municipio",
                table: "Farms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Farms",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Departamento",
                table: "Farms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<decimal>(
                name: "AreaManzanas",
                table: "Farms",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoInvitacion",
                table: "Companies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Crops",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visits",
                table: "Visits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistanceRequestCrops",
                table: "AssistanceRequestCrops",
                columns: new[] { "SolicitudId", "CultivoId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistanceRequests",
                table: "AssistanceRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Producers",
                table: "Producers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Farms",
                table: "Farms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Crops",
                table: "Crops",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_FincaId",
                table: "Visits",
                column: "FincaId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_TecnicoId",
                table: "Visits",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequests_ProductorId",
                table: "AssistanceRequests",
                column: "ProductorId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ProductorId",
                table: "Farms",
                column: "ProductorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequestCrops_AssistanceRequests_SolicitudId",
                table: "AssistanceRequestCrops",
                column: "SolicitudId",
                principalTable: "AssistanceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequestCrops_Crops_CultivoId",
                table: "AssistanceRequestCrops",
                column: "CultivoId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequests_Farms_FincaId",
                table: "AssistanceRequests",
                column: "FincaId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequests_Producers_ProductorId",
                table: "AssistanceRequests",
                column: "ProductorId",
                principalTable: "Producers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_Farms_FincaId",
                table: "Crops",
                column: "FincaId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_Producers_ProductorId",
                table: "Farms",
                column: "ProductorId",
                principalTable: "Producers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_AssistanceRequests_SolicitudId",
                table: "Visits",
                column: "SolicitudId",
                principalTable: "AssistanceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Farms_FincaId",
                table: "Visits",
                column: "FincaId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Technicians_TecnicoId",
                table: "Visits",
                column: "TecnicoId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
