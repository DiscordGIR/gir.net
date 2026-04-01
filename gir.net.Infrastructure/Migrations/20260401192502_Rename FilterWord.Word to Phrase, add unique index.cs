using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gir.net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameFilterWordWordtoPhraseadduniqueindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "word",
                table: "filter_words",
                newName: "phrase");

            migrationBuilder.CreateIndex(
                name: "ix_filter_words_phrase",
                table: "filter_words",
                column: "phrase",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_filter_words_phrase",
                table: "filter_words");

            migrationBuilder.RenameColumn(
                name: "phrase",
                table: "filter_words",
                newName: "word");
        }
    }
}
