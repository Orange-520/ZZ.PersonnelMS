using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckUserByRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records");

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
                name: "CheckUserId",
                table: "T_Records",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_T_Records_CheckUserId",
                table: "T_Records",
                column: "CheckUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Users_CheckUserId",
                table: "T_Records",
                column: "CheckUserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Users_CheckUserId",
                table: "T_Records");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records");

            migrationBuilder.DropIndex(
                name: "IX_T_Records_CheckUserId",
                table: "T_Records");

            migrationBuilder.DropColumn(
                name: "CheckUserId",
                table: "T_Records");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "T_Records",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Records_T_Users_UserId",
                table: "T_Records",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id");
        }
    }
}
