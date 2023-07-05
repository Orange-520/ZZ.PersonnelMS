using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignDepartmentWithPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmenId",
                table: "T_Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmenId",
                table: "T_Departments",
                column: "ParentDepartmenId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmenId",
                table: "T_Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmenId",
                table: "T_Departments",
                column: "ParentDepartmenId",
                principalTable: "T_Departments",
                principalColumn: "Id");
        }
    }
}
