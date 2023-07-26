using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentFolderID",
                table: "FolderInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FolderInfo_ParentFolderID",
                table: "FolderInfo",
                column: "ParentFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_FolderInfo_FolderInfo_ParentFolderID",
                table: "FolderInfo",
                column: "ParentFolderID",
                principalTable: "FolderInfo",
                principalColumn: "FolderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FolderInfo_FolderInfo_ParentFolderID",
                table: "FolderInfo");

            migrationBuilder.DropIndex(
                name: "IX_FolderInfo_ParentFolderID",
                table: "FolderInfo");

            migrationBuilder.DropColumn(
                name: "ParentFolderID",
                table: "FolderInfo");
        }
    }
}
