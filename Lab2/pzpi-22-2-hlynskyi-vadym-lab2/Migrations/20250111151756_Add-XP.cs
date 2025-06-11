using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUp.Migrations
{
    /// <inheritdoc />
    public partial class AddXP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XP",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XP",
                table: "Users");
        }
    }
}
