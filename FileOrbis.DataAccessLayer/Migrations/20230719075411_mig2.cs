using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "FolderInfo",
                newName: "FolderCreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "FileInfo",
                newName: "FileCreationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolderCreatedDate",
                table: "FolderInfo",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "FileCreationDate",
                table: "FileInfo",
                newName: "CreationDate");
        }
    }
}
