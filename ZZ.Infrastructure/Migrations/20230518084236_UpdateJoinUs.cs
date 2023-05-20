using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJoinUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Certificates_T_Users_UserId",
                table: "T_Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_T_EducationHistorys_T_Users_UserId",
                table: "T_EducationHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records");

            migrationBuilder.DropIndex(
                name: "IX_T_EducationHistorys_UserId",
                table: "T_EducationHistorys");

            migrationBuilder.DropIndex(
                name: "IX_T_Certificates_UserId",
                table: "T_Certificates");

            migrationBuilder.DropColumn(
                name: "PersonalInformationId",
                table: "T_WorkHistorys");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "T_EducationHistorys");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "T_Certificates");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "T_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "T_Records",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "RecordId",
                table: "T_Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResumeId",
                table: "T_Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_Certificates_RecordId",
                table: "T_Certificates",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Certificates_ResumeId",
                table: "T_Certificates",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Certificates_T_Records_RecordId",
                table: "T_Certificates",
                column: "RecordId",
                principalTable: "T_Records",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Certificates_T_Resumes_ResumeId",
                table: "T_Certificates",
                column: "ResumeId",
                principalTable: "T_Resumes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Certificates_T_Records_RecordId",
                table: "T_Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Certificates_T_Resumes_ResumeId",
                table: "T_Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records");

            migrationBuilder.DropIndex(
                name: "IX_T_Certificates_RecordId",
                table: "T_Certificates");

            migrationBuilder.DropIndex(
                name: "IX_T_Certificates_ResumeId",
                table: "T_Certificates");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "T_Users");

            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "T_Certificates");

            migrationBuilder.DropColumn(
                name: "ResumeId",
                table: "T_Certificates");

            migrationBuilder.AddColumn<int>(
                name: "PersonalInformationId",
                table: "T_WorkHistorys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "T_Records",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "T_EducationHistorys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "T_Certificates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_T_EducationHistorys_UserId",
                table: "T_EducationHistorys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Certificates_UserId",
                table: "T_Certificates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Certificates_T_Users_UserId",
                table: "T_Certificates",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_EducationHistorys_T_Users_UserId",
                table: "T_EducationHistorys",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
