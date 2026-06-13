using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace gir.net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Createcasesentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    mod_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    mod_tag = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    punishment = table.Column<string>(type: "text", nullable: false),
                    lifted = table.Column<bool>(type: "boolean", nullable: false),
                    lifted_by_tag = table.Column<string>(type: "text", nullable: true),
                    lifted_by_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    lifted_reason = table.Column<string>(type: "text", nullable: true),
                    lifted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cases", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cases_user_id",
                table: "cases",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cases");
        }
    }
}
