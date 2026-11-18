using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AgroCita.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelAfterInitialCreate : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_Visits_FincaId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_SolicitudId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_TecnicoId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ProductorId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Crops_FincaId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequests_FincaId",
                table: "AssistanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequests_ProductorId",
                table: "AssistanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequestCrops_CultivoId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

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
                name: "Rol",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ProductorId",
                table: "AssistanceRequests");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "empresas");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Visits",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "NombreCompleto",
                table: "usuarios",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Dni",
                table: "Producers",
                newName: "DNI");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AssistanceRequests",
                newName: "FechaCreacion");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaProgramada",
                table: "Visits",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Visits",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelacion",
                table: "Visits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "usuarios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Technicians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Producers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AreaManzanas",
                table: "Farms",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Farms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Farms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Crops",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "AreaSembrada",
                table: "Crops",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Crops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaSiembra",
                table: "Crops",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoCultivoId",
                table: "Crops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Variedad",
                table: "Crops",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "AssistanceRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "ComentarioTecnico",
                table: "AssistanceRequests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "AssistanceRequestCrops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "empresas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "empresas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RTN",
                table: "empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Visits_Id_EmpresaId",
                table: "Visits",
                columns: new[] { "Id", "EmpresaId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Technicians_Id_EmpresaId",
                table: "Technicians",
                columns: new[] { "Id", "EmpresaId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Producers_Id_EmpresaId",
                table: "Producers",
                columns: new[] { "Id", "EmpresaId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Farms_Id_EmpresaId",
                table: "Farms",
                columns: new[] { "Id", "EmpresaId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Crops_Id_EmpresaId",
                table: "Crops",
                columns: new[] { "Id", "EmpresaId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AssistanceRequests_Id_EmpresaId",
                table: "AssistanceRequests",
                columns: new[] { "Id", "EmpresaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_empresas",
                table: "empresas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Fertilizers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Composicion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fertilizers", x => x.Id);
                    table.UniqueConstraint("AK_Fertilizers_Id_EmpresaId", x => new { x.Id, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_Fertilizers_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualityCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityCategories", x => x.Id);
                    table.UniqueConstraint("AK_QualityCategories_Id_EmpresaId", x => new { x.Id, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_QualityCategories_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    Diagnostico = table.Column<string>(type: "text", nullable: true),
                    Recomendaciones = table.Column<string>(type: "text", nullable: true),
                    FechaElaboracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Visits_VisitaId_EmpresaId",
                        columns: x => new { x.VisitaId, x.EmpresaId },
                        principalTable: "Visits",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReschedulingVisits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    FechaAnterior = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaNueva = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Motivo = table.Column<string>(type: "text", nullable: false),
                    FechaReprogramacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReschedulingVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReschedulingVisits_Visits_VisitaId_EmpresaId",
                        columns: x => new { x.VisitaId, x.EmpresaId },
                        principalTable: "Visits",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReschedulingVisits_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReschedulingVisits_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    PrecioMensual = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxTecnicos = table.Column<int>(type: "integer", nullable: false),
                    MaxProductores = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposCultivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposCultivo", x => x.Id);
                    table.UniqueConstraint("AK_TiposCultivo_Id_EmpresaId", x => new { x.Id, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_TiposCultivo_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    FertilizanteId = table.Column<int>(type: "integer", nullable: false),
                    Dosis = table.Column<decimal>(type: "numeric", nullable: false),
                    UnidadMedida = table.Column<string>(type: "text", nullable: false),
                    Frecuencia = table.Column<string>(type: "text", nullable: true),
                    Indicaciones = table.Column<string>(type: "text", nullable: true),
                    FechaRecomendacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerRecommendations", x => x.Id);
                    table.UniqueConstraint("AK_FertilizerRecommendations_Id_EmpresaId", x => new { x.Id, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_FertilizerRecommendations_Crops_CultivoId_EmpresaId",
                        columns: x => new { x.CultivoId, x.EmpresaId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerRecommendations_Fertilizers_FertilizanteId_Empres~",
                        columns: x => new { x.FertilizanteId, x.EmpresaId },
                        principalTable: "Fertilizers",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerRecommendations_Visits_VisitaId_EmpresaId",
                        columns: x => new { x.VisitaId, x.EmpresaId },
                        principalTable: "Visits",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerRecommendations_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    VisitaId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaCalidadId = table.Column<int>(type: "integer", nullable: false),
                    FechaInspeccion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    QualityCategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.UniqueConstraint("AK_Inspections_Id_EmpresaId", x => new { x.Id, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_Inspections_Crops_CultivoId_EmpresaId",
                        columns: x => new { x.CultivoId, x.EmpresaId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_QualityCategories_CategoriaCalidadId_EmpresaId",
                        columns: x => new { x.CategoriaCalidadId, x.EmpresaId },
                        principalTable: "QualityCategories",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_QualityCategories_QualityCategoryId",
                        column: x => x.QualityCategoryId,
                        principalTable: "QualityCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inspections_Visits_VisitaId_EmpresaId",
                        columns: x => new { x.VisitaId, x.EmpresaId },
                        principalTable: "Visits",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    TipoCultivoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Unidad = table.Column<string>(type: "text", nullable: false),
                    ValorMinimo = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorMaximo = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationParameters", x => x.Id);
                    table.UniqueConstraint("AK_EvaluationParameters_Id_EmpresaId", x => new { x.Id, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_EvaluationParameters_TiposCultivo_TipoCultivoId_EmpresaId",
                        columns: x => new { x.TipoCultivoId, x.EmpresaId },
                        principalTable: "TiposCultivo",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationParameters_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    FertilizanteId = table.Column<int>(type: "integer", nullable: false),
                    RecomendacionId = table.Column<int>(type: "integer", nullable: false),
                    TecnicoId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric", nullable: false),
                    UnidadMedida = table.Column<string>(type: "text", nullable: false),
                    FechaAplicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FertilizerApplications_Crops_CultivoId_EmpresaId",
                        columns: x => new { x.CultivoId, x.EmpresaId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerApplications_FertilizerRecommendations_Recomendac~",
                        columns: x => new { x.RecomendacionId, x.EmpresaId },
                        principalTable: "FertilizerRecommendations",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerApplications_Fertilizers_FertilizanteId_EmpresaId",
                        columns: x => new { x.FertilizanteId, x.EmpresaId },
                        principalTable: "Fertilizers",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerApplications_Technicians_TecnicoId_EmpresaId",
                        columns: x => new { x.TecnicoId, x.EmpresaId },
                        principalTable: "Technicians",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FertilizerApplications_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParameterResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    InspeccionId = table.Column<int>(type: "integer", nullable: false),
                    ParametroEvaluacionId = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<string>(type: "text", nullable: false),
                    EvaluationParameterId = table.Column<int>(type: "integer", nullable: true),
                    InspectionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterResults_EvaluationParameters_EvaluationParameterId",
                        column: x => x.EvaluationParameterId,
                        principalTable: "EvaluationParameters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParameterResults_EvaluationParameters_ParametroEvaluacionId~",
                        columns: x => new { x.ParametroEvaluacionId, x.EmpresaId },
                        principalTable: "EvaluationParameters",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParameterResults_Inspections_InspeccionId_EmpresaId",
                        columns: x => new { x.InspeccionId, x.EmpresaId },
                        principalTable: "Inspections",
                        principalColumns: new[] { "Id", "EmpresaId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParameterResults_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParameterResults_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_EmpresaId",
                table: "Visits",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_SolicitudId_EmpresaId",
                table: "Visits",
                columns: new[] { "SolicitudId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_TecnicoId_EmpresaId",
                table: "Visits",
                columns: new[] { "TecnicoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_EmpresaId",
                table: "usuarios",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_RolId",
                table: "usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_EmpresaId",
                table: "Technicians",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_UsuarioId",
                table: "Technicians",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producers_EmpresaId",
                table: "Producers",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producers_UsuarioId",
                table: "Producers",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_EmpresaId",
                table: "Farms",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ProductorId_EmpresaId",
                table: "Farms",
                columns: new[] { "ProductorId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Crops_EmpresaId",
                table: "Crops",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_FincaId_EmpresaId",
                table: "Crops",
                columns: new[] { "FincaId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Crops_TipoCultivoId_EmpresaId",
                table: "Crops",
                columns: new[] { "TipoCultivoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequests_EmpresaId",
                table: "AssistanceRequests",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequests_FincaId_EmpresaId",
                table: "AssistanceRequests",
                columns: new[] { "FincaId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequestCrops_CultivoId_EmpresaId",
                table: "AssistanceRequestCrops",
                columns: new[] { "CultivoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequestCrops_EmpresaId",
                table: "AssistanceRequestCrops",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequestCrops_SolicitudId_EmpresaId",
                table: "AssistanceRequestCrops",
                columns: new[] { "SolicitudId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParameters_EmpresaId",
                table: "EvaluationParameters",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParameters_TipoCultivoId_EmpresaId",
                table: "EvaluationParameters",
                columns: new[] { "TipoCultivoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplications_CultivoId_EmpresaId",
                table: "FertilizerApplications",
                columns: new[] { "CultivoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplications_EmpresaId",
                table: "FertilizerApplications",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplications_FertilizanteId_EmpresaId",
                table: "FertilizerApplications",
                columns: new[] { "FertilizanteId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplications_RecomendacionId_EmpresaId",
                table: "FertilizerApplications",
                columns: new[] { "RecomendacionId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplications_TecnicoId_EmpresaId",
                table: "FertilizerApplications",
                columns: new[] { "TecnicoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerRecommendations_CultivoId_EmpresaId",
                table: "FertilizerRecommendations",
                columns: new[] { "CultivoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerRecommendations_EmpresaId",
                table: "FertilizerRecommendations",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerRecommendations_FertilizanteId_EmpresaId",
                table: "FertilizerRecommendations",
                columns: new[] { "FertilizanteId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerRecommendations_VisitaId_EmpresaId",
                table: "FertilizerRecommendations",
                columns: new[] { "VisitaId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Fertilizers_EmpresaId",
                table: "Fertilizers",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_CategoriaCalidadId_EmpresaId",
                table: "Inspections",
                columns: new[] { "CategoriaCalidadId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_CultivoId_EmpresaId",
                table: "Inspections",
                columns: new[] { "CultivoId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_EmpresaId",
                table: "Inspections",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_QualityCategoryId",
                table: "Inspections",
                column: "QualityCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_VisitaId_EmpresaId",
                table: "Inspections",
                columns: new[] { "VisitaId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_EmpresaId",
                table: "ParameterResults",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_EvaluationParameterId",
                table: "ParameterResults",
                column: "EvaluationParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_InspeccionId_EmpresaId",
                table: "ParameterResults",
                columns: new[] { "InspeccionId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_InspectionId",
                table: "ParameterResults",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_ParametroEvaluacionId_EmpresaId",
                table: "ParameterResults",
                columns: new[] { "ParametroEvaluacionId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_QualityCategories_EmpresaId",
                table: "QualityCategories",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_EmpresaId",
                table: "Reports",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_VisitaId_EmpresaId",
                table: "Reports",
                columns: new[] { "VisitaId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReschedulingVisits_EmpresaId",
                table: "ReschedulingVisits",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReschedulingVisits_UsuarioId",
                table: "ReschedulingVisits",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReschedulingVisits_VisitaId_EmpresaId",
                table: "ReschedulingVisits",
                columns: new[] { "VisitaId", "EmpresaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_EmpresaId",
                table: "Subscriptions",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposCultivo_EmpresaId",
                table: "TiposCultivo",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequestCrops_AssistanceRequests_SolicitudId_Empre~",
                table: "AssistanceRequestCrops",
                columns: new[] { "SolicitudId", "EmpresaId" },
                principalTable: "AssistanceRequests",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequestCrops_Crops_CultivoId_EmpresaId",
                table: "AssistanceRequestCrops",
                columns: new[] { "CultivoId", "EmpresaId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequestCrops_empresas_EmpresaId",
                table: "AssistanceRequestCrops",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequests_Farms_FincaId_EmpresaId",
                table: "AssistanceRequests",
                columns: new[] { "FincaId", "EmpresaId" },
                principalTable: "Farms",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssistanceRequests_empresas_EmpresaId",
                table: "AssistanceRequests",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_Farms_FincaId_EmpresaId",
                table: "Crops",
                columns: new[] { "FincaId", "EmpresaId" },
                principalTable: "Farms",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_TiposCultivo_TipoCultivoId_EmpresaId",
                table: "Crops",
                columns: new[] { "TipoCultivoId", "EmpresaId" },
                principalTable: "TiposCultivo",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_empresas_EmpresaId",
                table: "Crops",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_Producers_ProductorId_EmpresaId",
                table: "Farms",
                columns: new[] { "ProductorId", "EmpresaId" },
                principalTable: "Producers",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_empresas_EmpresaId",
                table: "Farms",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Producers_empresas_EmpresaId",
                table: "Producers",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Producers_usuarios_UsuarioId",
                table: "Producers",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_empresas_EmpresaId",
                table: "Technicians",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_usuarios_UsuarioId",
                table: "Technicians",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_empresas_EmpresaId",
                table: "usuarios",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_roles_RolId",
                table: "usuarios",
                column: "RolId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_AssistanceRequests_SolicitudId_EmpresaId",
                table: "Visits",
                columns: new[] { "SolicitudId", "EmpresaId" },
                principalTable: "AssistanceRequests",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Technicians_TecnicoId_EmpresaId",
                table: "Visits",
                columns: new[] { "TecnicoId", "EmpresaId" },
                principalTable: "Technicians",
                principalColumns: new[] { "Id", "EmpresaId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_empresas_EmpresaId",
                table: "Visits",
                column: "EmpresaId",
                principalTable: "empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequestCrops_AssistanceRequests_SolicitudId_Empre~",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequestCrops_Crops_CultivoId_EmpresaId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequestCrops_empresas_EmpresaId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequests_Farms_FincaId_EmpresaId",
                table: "AssistanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AssistanceRequests_empresas_EmpresaId",
                table: "AssistanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_Farms_FincaId_EmpresaId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_TiposCultivo_TipoCultivoId_EmpresaId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_empresas_EmpresaId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_Producers_ProductorId_EmpresaId",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_empresas_EmpresaId",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_Producers_empresas_EmpresaId",
                table: "Producers");

            migrationBuilder.DropForeignKey(
                name: "FK_Producers_usuarios_UsuarioId",
                table: "Producers");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_empresas_EmpresaId",
                table: "Technicians");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_usuarios_UsuarioId",
                table: "Technicians");

            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_empresas_EmpresaId",
                table: "usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_roles_RolId",
                table: "usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_AssistanceRequests_SolicitudId_EmpresaId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Technicians_TecnicoId_EmpresaId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_empresas_EmpresaId",
                table: "Visits");

            migrationBuilder.DropTable(
                name: "FertilizerApplications");

            migrationBuilder.DropTable(
                name: "ParameterResults");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ReschedulingVisits");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "FertilizerRecommendations");

            migrationBuilder.DropTable(
                name: "EvaluationParameters");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "Fertilizers");

            migrationBuilder.DropTable(
                name: "TiposCultivo");

            migrationBuilder.DropTable(
                name: "QualityCategories");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Visits_Id_EmpresaId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_EmpresaId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_SolicitudId_EmpresaId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_TecnicoId_EmpresaId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_EmpresaId",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_RolId",
                table: "usuarios");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Technicians_Id_EmpresaId",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_EmpresaId",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_UsuarioId",
                table: "Technicians");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Producers_Id_EmpresaId",
                table: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Producers_EmpresaId",
                table: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Producers_UsuarioId",
                table: "Producers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Farms_Id_EmpresaId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_EmpresaId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ProductorId_EmpresaId",
                table: "Farms");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Crops_Id_EmpresaId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_EmpresaId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_FincaId_EmpresaId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_TipoCultivoId_EmpresaId",
                table: "Crops");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AssistanceRequests_Id_EmpresaId",
                table: "AssistanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequests_EmpresaId",
                table: "AssistanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequests_FincaId_EmpresaId",
                table: "AssistanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequestCrops_CultivoId_EmpresaId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequestCrops_EmpresaId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceRequestCrops_SolicitudId_EmpresaId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_empresas",
                table: "empresas");

            migrationBuilder.DropColumn(
                name: "MotivoCancelacion",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "AreaSembrada",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "FechaSiembra",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "TipoCultivoId",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "Variedad",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "ComentarioTecnico",
                table: "AssistanceRequests");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "AssistanceRequestCrops");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "empresas");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "empresas");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "empresas");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "empresas");

            migrationBuilder.DropColumn(
                name: "RTN",
                table: "empresas");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "empresas");

            migrationBuilder.RenameTable(
                name: "empresas",
                newName: "Companies");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Visits",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "usuarios",
                newName: "NombreCompleto");

            migrationBuilder.RenameColumn(
                name: "DNI",
                table: "Producers",
                newName: "Dni");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "AssistanceRequests",
                newName: "CreatedAt");

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
                oldType: "text");

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

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "usuarios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Technicians",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Producers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.AlterColumn<decimal>(
                name: "AreaManzanas",
                table: "Farms",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "Farms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "AssistanceRequests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ProductorId",
                table: "AssistanceRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_FincaId",
                table: "Visits",
                column: "FincaId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_SolicitudId",
                table: "Visits",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_TecnicoId",
                table: "Visits",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ProductorId",
                table: "Farms",
                column: "ProductorId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_FincaId",
                table: "Crops",
                column: "FincaId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequests_FincaId",
                table: "AssistanceRequests",
                column: "FincaId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequests_ProductorId",
                table: "AssistanceRequests",
                column: "ProductorId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceRequestCrops_CultivoId",
                table: "AssistanceRequestCrops",
                column: "CultivoId");

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
