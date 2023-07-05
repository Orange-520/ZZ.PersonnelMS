using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteNonexistentOfDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Departments_T_Departments_DepartmentId",
                table: "T_Departments");

            migrationBuilder.DropIndex(
                name: "IX_T_Departments_DepartmentId",
                table: "T_Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "T_Departments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "T_Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_Departments_DepartmentId",
                table: "T_Departments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Departments_T_Departments_DepartmentId",
                table: "T_Departments",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id");
        }
    }
}
