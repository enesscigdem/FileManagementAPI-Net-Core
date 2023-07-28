using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolderPath",
                table: "FolderInfo",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "FolderCreatedDate",
                table: "FolderInfo",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "FileInfo",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "FileInfo",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "FileCreationDate",
                table: "FileInfo",
                newName: "CreationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "FolderInfo",
                newName: "FolderPath");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "FolderInfo",
                newName: "FolderCreatedDate");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "FileInfo",
                newName: "FileSize");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "FileInfo",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "FileInfo",
                newName: "FileCreationDate");
        }
    }
}
