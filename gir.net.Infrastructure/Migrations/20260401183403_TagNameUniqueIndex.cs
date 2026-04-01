using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gir.net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TagNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                table: "tags",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tags_name",
                table: "tags");
        }
    }
}
