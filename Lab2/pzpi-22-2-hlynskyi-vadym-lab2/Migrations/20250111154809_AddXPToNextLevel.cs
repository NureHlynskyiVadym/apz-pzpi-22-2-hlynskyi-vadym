using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUp.Migrations
{
    /// <inheritdoc />
    public partial class AddXPToNextLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XPToNextLevel",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPToNextLevel",
                table: "Users");
        }
    }
}
