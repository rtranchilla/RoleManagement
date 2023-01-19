using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleManagement.RoleManagementService.DataPersistence.Migrations
{
    public partial class p : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleRequiredNodes",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRequiredNodes", x => new { x.RoleId, x.NodeId });
                    table.ForeignKey(
                        name: "FK_RoleRequiredNodes_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleRequiredNodes_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeRequiredNodes",
                columns: table => new
                {
                    TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeRequiredNodes", x => new { x.TreeId, x.NodeId });
                    table.ForeignKey(
                        name: "FK_TreeRequiredNodes_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreeRequiredNodes_Trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequiredNodes_NodeId",
                table: "RoleRequiredNodes",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeRequiredNodes_NodeId",
                table: "TreeRequiredNodes",
                column: "NodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleRequiredNodes");

            migrationBuilder.DropTable(
                name: "TreeRequiredNodes");
        }
    }
}
