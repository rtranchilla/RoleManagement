using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleConfiguration.DataPersistence.Migrations;

public partial class Inital : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Members",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UniqueName = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Sources",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Trees",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Trees", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Files",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Path = table.Column<string>(type: "nvarchar(450)", nullable: false),
                SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Files", x => x.Id);
                table.ForeignKey(
                    name: "FK_Files_Sources_SourceId",
                    column: x => x.SourceId,
                    principalTable: "Sources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
                table.ForeignKey(
                    name: "FK_Roles_Trees_TreeId",
                    column: x => x.TreeId,
                    principalTable: "Trees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FileMembers",
            columns: table => new
            {
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FileMembers", x => new { x.FileId, x.MemberId });
                table.ForeignKey(
                    name: "FK_FileMembers_Files_FileId",
                    column: x => x.FileId,
                    principalTable: "Files",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_FileMembers_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FileTrees",
            columns: table => new
            {
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FileTrees", x => new { x.FileId, x.TreeId });
                table.ForeignKey(
                    name: "FK_FileTrees_Files_FileId",
                    column: x => x.FileId,
                    principalTable: "Files",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_FileTrees_Trees_TreeId",
                    column: x => x.TreeId,
                    principalTable: "Trees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FileRoles",
            columns: table => new
            {
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FileRoles", x => new { x.FileId, x.RoleId });
                table.ForeignKey(
                    name: "FK_FileRoles_Files_FileId",
                    column: x => x.FileId,
                    principalTable: "Files",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_FileRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_FileMembers_MemberId",
            table: "FileMembers",
            column: "MemberId");

        migrationBuilder.CreateIndex(
            name: "IX_FileRoles_RoleId",
            table: "FileRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Files_Path_SourceId",
            table: "Files",
            columns: new[] { "Path", "SourceId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Files_SourceId",
            table: "Files",
            column: "SourceId");

        migrationBuilder.CreateIndex(
            name: "IX_FileTrees_TreeId",
            table: "FileTrees",
            column: "TreeId");

        migrationBuilder.CreateIndex(
            name: "IX_Members_UniqueName",
            table: "Members",
            column: "UniqueName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Roles_Name_TreeId",
            table: "Roles",
            columns: new[] { "Name", "TreeId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Roles_TreeId",
            table: "Roles",
            column: "TreeId");

        migrationBuilder.CreateIndex(
            name: "IX_Sources_Name",
            table: "Sources",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Trees_Name",
            table: "Trees",
            column: "Name",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FileMembers");

        migrationBuilder.DropTable(
            name: "FileRoles");

        migrationBuilder.DropTable(
            name: "FileTrees");

        migrationBuilder.DropTable(
            name: "Members");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.DropTable(
            name: "Files");

        migrationBuilder.DropTable(
            name: "Trees");

        migrationBuilder.DropTable(
            name: "Sources");
    }
}
