using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FolderInfo_FolderInfo_ParentFolderID",
                table: "FolderInfo");

            migrationBuilder.DropIndex(
                name: "IX_FolderInfo_ParentFolderID",
                table: "FolderInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
