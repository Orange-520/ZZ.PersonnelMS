using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckUserByResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CheckUserId",
                table: "T_Resumes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_Resumes_CheckUserId",
                table: "T_Resumes",
                column: "CheckUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Resumes_T_Users_CheckUserId",
                table: "T_Resumes",
                column: "CheckUserId",
                principalTable: "T_Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Resumes_T_Users_CheckUserId",
                table: "T_Resumes");

            migrationBuilder.DropIndex(
                name: "IX_T_Resumes_CheckUserId",
                table: "T_Resumes");

            migrationBuilder.DropColumn(
                name: "CheckUserId",
                table: "T_Resumes");
        }
    }
}
