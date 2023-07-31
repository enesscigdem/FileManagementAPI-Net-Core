using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileInfo_FolderInfo_FolderID",
                table: "FileInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_FolderInfo_UserInfo_UserID",
                table: "FolderInfo");

            migrationBuilder.DropIndex(
                name: "IX_FolderInfo_UserID",
                table: "FolderInfo");

            migrationBuilder.DropIndex(
                name: "IX_FileInfo_FolderID",
                table: "FileInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FolderInfo_UserID",
                table: "FolderInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FileInfo_FolderID",
                table: "FileInfo",
                column: "FolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfo_FolderInfo_FolderID",
                table: "FileInfo",
                column: "FolderID",
                principalTable: "FolderInfo",
                principalColumn: "FolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_FolderInfo_UserInfo_UserID",
                table: "FolderInfo",
                column: "UserID",
                principalTable: "UserInfo",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
