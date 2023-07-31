using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileInfo_FolderInfo_FolderID",
                table: "FileInfo");

            migrationBuilder.AlterColumn<int>(
                name: "FolderID",
                table: "FileInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfo_FolderInfo_FolderID",
                table: "FileInfo",
                column: "FolderID",
                principalTable: "FolderInfo",
                principalColumn: "FolderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileInfo_FolderInfo_FolderID",
                table: "FileInfo");

            migrationBuilder.AlterColumn<int>(
                name: "FolderID",
                table: "FileInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfo_FolderInfo_FolderID",
                table: "FileInfo",
                column: "FolderID",
                principalTable: "FolderInfo",
                principalColumn: "FolderID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
