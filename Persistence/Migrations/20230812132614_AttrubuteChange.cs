using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AttrubuteChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoInASeries",
                table: "Series");

            migrationBuilder.AddColumn<int>(
                name: "NoInASeries",
                table: "BookSeries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoInASeries",
                table: "BookSeries");

            migrationBuilder.AddColumn<int>(
                name: "NoInASeries",
                table: "Series",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
