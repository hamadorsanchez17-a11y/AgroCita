using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroCita.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixDuplicateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_QualityCategories_QualityCategoryId",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterResults_EvaluationParameters_EvaluationParameterId",
                table: "ParameterResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterResults_Inspections_InspectionId",
                table: "ParameterResults");

            migrationBuilder.DropIndex(
                name: "IX_ParameterResults_EvaluationParameterId",
                table: "ParameterResults");

            migrationBuilder.DropIndex(
                name: "IX_ParameterResults_InspectionId",
                table: "ParameterResults");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_QualityCategoryId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "EvaluationParameterId",
                table: "ParameterResults");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "ParameterResults");

            migrationBuilder.DropColumn(
                name: "QualityCategoryId",
                table: "Inspections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvaluationParameterId",
                table: "ParameterResults",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectionId",
                table: "ParameterResults",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QualityCategoryId",
                table: "Inspections",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_EvaluationParameterId",
                table: "ParameterResults",
                column: "EvaluationParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterResults_InspectionId",
                table: "ParameterResults",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_QualityCategoryId",
                table: "Inspections",
                column: "QualityCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_QualityCategories_QualityCategoryId",
                table: "Inspections",
                column: "QualityCategoryId",
                principalTable: "QualityCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterResults_EvaluationParameters_EvaluationParameterId",
                table: "ParameterResults",
                column: "EvaluationParameterId",
                principalTable: "EvaluationParameters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterResults_Inspections_InspectionId",
                table: "ParameterResults",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id");
        }
    }
}
