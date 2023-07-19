using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileInfo");

            migrationBuilder.DropTable(
                name: "FolderInfo");

            migrationBuilder.CreateTable(
                name: "FileItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFolder = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    ParentFolderId = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileItems_FileItems_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "FileItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileItems_UserInfo_UserID",
                        column: x => x.UserID,
                        principalTable: "UserInfo",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileItems_ParentFolderId",
                table: "FileItems",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FileItems_UserID",
                table: "FileItems",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileItems");

            migrationBuilder.CreateTable(
                name: "FolderInfo",
                columns: table => new
                {
                    FolderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FolderCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FolderPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderInfo", x => x.FolderID);
                    table.ForeignKey(
                        name: "FK_FolderInfo_UserInfo_UserID",
                        column: x => x.UserID,
                        principalTable: "UserInfo",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileInfo",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderID = table.Column<int>(type: "int", nullable: false),
                    FileCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfo", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_FileInfo_FolderInfo_FolderID",
                        column: x => x.FolderID,
                        principalTable: "FolderInfo",
                        principalColumn: "FolderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileInfo_FolderID",
                table: "FileInfo",
                column: "FolderID");

            migrationBuilder.CreateIndex(
                name: "IX_FolderInfo_UserID",
                table: "FolderInfo",
                column: "UserID");
        }
    }
}
