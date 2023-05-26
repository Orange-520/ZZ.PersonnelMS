using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyReplyTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyTitle",
                table: "T_Replys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyTitle",
                table: "T_Replys");
        }
    }
}
