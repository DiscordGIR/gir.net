using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gir.net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TagImageUrlsJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "image_urls",
                table: "tags",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'::text[]");

            migrationBuilder.Sql(
                """
                UPDATE tags
                SET image_urls = ARRAY[image_url]::text[]
                WHERE image_url IS NOT NULL;
                """);

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "tags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "tags",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE tags
                SET image_url = image_urls[1]
                WHERE cardinality(image_urls) > 0;
                """);

            migrationBuilder.DropColumn(
                name: "image_urls",
                table: "tags");
        }
    }
}
