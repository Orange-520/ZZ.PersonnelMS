using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDepartmentAndPositionWithItOfForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_HiringNeedApplys_T_Departments_DepartmentId",
                table: "T_HiringNeedApplys");

            migrationBuilder.DropForeignKey(
                name: "FK_T_HiringNeedApplys_T_Positions_PositionId",
                table: "T_HiringNeedApplys");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Departments_DepartmentId",
                table: "T_Records");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Positions_PositionId",
                table: "T_Records");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Resumes_T_Departments_DepartmentId",
                table: "T_Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Resumes_T_Positions_PositionId",
                table: "T_Resumes");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "T_Resumes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "T_Resumes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "T_Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "T_Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "T_HiringNeedApplys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "T_HiringNeedApplys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_T_HiringNeedApplys_T_Departments_DepartmentId",
                table: "T_HiringNeedApplys",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_HiringNeedApplys_T_Positions_PositionId",
                table: "T_HiringNeedApplys",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Departments_DepartmentId",
                table: "T_Records",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Positions_PositionId",
                table: "T_Records",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Resumes_T_Departments_DepartmentId",
                table: "T_Resumes",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Resumes_T_Positions_PositionId",
                table: "T_Resumes",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Departments_T_Departments_DepartmentId",
                table: "T_Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_T_HiringNeedApplys_T_Departments_DepartmentId",
                table: "T_HiringNeedApplys");

            migrationBuilder.DropForeignKey(
                name: "FK_T_HiringNeedApplys_T_Positions_PositionId",
                table: "T_HiringNeedApplys");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Departments_DepartmentId",
                table: "T_Records");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Positions_PositionId",
                table: "T_Records");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Resumes_T_Departments_DepartmentId",
                table: "T_Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Resumes_T_Positions_PositionId",
                table: "T_Resumes");

            migrationBuilder.DropIndex(
                name: "IX_T_Departments_DepartmentId",
                table: "T_Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "T_Departments");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "T_Resumes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "T_Resumes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "T_Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "T_Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "T_HiringNeedApplys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "T_HiringNeedApplys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T_HiringNeedApplys_T_Departments_DepartmentId",
                table: "T_HiringNeedApplys",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_HiringNeedApplys_T_Positions_PositionId",
                table: "T_HiringNeedApplys",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Departments_DepartmentId",
                table: "T_Records",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Positions_PositionId",
                table: "T_Records",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Resumes_T_Departments_DepartmentId",
                table: "T_Resumes",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Resumes_T_Positions_PositionId",
                table: "T_Resumes",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
