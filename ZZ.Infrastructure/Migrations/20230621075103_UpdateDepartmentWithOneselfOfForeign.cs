using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDepartmentWithOneselfOfForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmenId",
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

            migrationBuilder.RenameColumn(
                name: "ParentDepartmenId",
                table: "T_Departments",
                newName: "ParentDepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Departments_ParentDepartmenId",
                table: "T_Departments",
                newName: "IX_T_Departments_ParentDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmentId",
                table: "T_Departments",
                column: "ParentDepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_T_HiringNeedApplys_T_Departments_DepartmentId",
                table: "T_HiringNeedApplys",
                column: "DepartmentId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                onDelete: ReferentialAction.Restrict);

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Resumes_T_Positions_PositionId",
                table: "T_Resumes",
                column: "PositionId",
                principalTable: "T_Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmentId",
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

            migrationBuilder.RenameColumn(
                name: "ParentDepartmentId",
                table: "T_Departments",
                newName: "ParentDepartmenId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Departments_ParentDepartmentId",
                table: "T_Departments",
                newName: "IX_T_Departments_ParentDepartmenId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Departments_T_Departments_ParentDepartmenId",
                table: "T_Departments",
                column: "ParentDepartmenId",
                principalTable: "T_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
