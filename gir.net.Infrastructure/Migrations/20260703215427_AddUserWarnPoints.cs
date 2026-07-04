using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gir.net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWarnPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "warn_points",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "was_warn_kicked",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "warn_points",
                table: "users");

            migrationBuilder.DropColumn(
                name: "was_warn_kicked",
                table: "users");
        }
    }
}
