using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnusedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookType_BookTypeId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "BookType");

            migrationBuilder.DropIndex(
                name: "IX_Book_BookTypeId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "BookTypeId",
                table: "Book");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookTypeId",
                table: "Book",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BookType",
                columns: table => new
                {
                    BookTypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookType", x => x.BookTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookTypeId",
                table: "Book",
                column: "BookTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookType_BookTypeId",
                table: "Book",
                column: "BookTypeId",
                principalTable: "BookType",
                principalColumn: "BookTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
