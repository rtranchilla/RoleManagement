using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleManagement.RoleManagementService.DataPersistence.Migrations
{
    public partial class o : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleNodes_Nodes_NodeId",
                table: "RoleNodes");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleNodes_Nodes_NodeId",
                table: "RoleNodes",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleNodes_Nodes_NodeId",
                table: "RoleNodes");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleNodes_Nodes_NodeId",
                table: "RoleNodes",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
