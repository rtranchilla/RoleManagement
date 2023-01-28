﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleManager.DataPersistence.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
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
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseNode = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_Trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reversible = table.Column<bool>(type: "bit", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "MemberRoles",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => new { x.MemberId, x.TreeId });
                    table.ForeignKey(
                        name: "FK_MemberRoles_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRoles_Trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleNodes",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleNodes", x => new { x.RoleId, x.NodeId });
                    table.ForeignKey(
                        name: "FK_RoleNodes_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleNodes_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_RoleId",
                table: "MemberRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_TreeId",
                table: "MemberRoles",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UniqueName",
                table: "Members",
                column: "UniqueName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_Name_TreeId",
                table: "Nodes",
                columns: new[] { "Name", "TreeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_TreeId",
                table: "Nodes",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleNodes_NodeId",
                table: "RoleNodes",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequiredNodes_NodeId",
                table: "RoleRequiredNodes",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TreeId",
                table: "Roles",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeRequiredNodes_NodeId",
                table: "TreeRequiredNodes",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trees_Name",
                table: "Trees",
                column: "Name",
                unique: true);

            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE [dbo].[sp_GetAvailableMemberRoles]
	@MemberId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	-- Get Member Nodes
	Declare
		@MemberNodes Table (
		[Id] UniqueIdentifier
	)

	Insert into
		@MemberNodes
	SELECT 
		rn.NodeId
	FROM
		[dbo].[MemberRoles] mr INNER JOIN
		[dbo].[RoleNodes] rn ON mr.RoleId = rn.RoleId
	Where
		mr.MemberId = @MemberId
	
	-- Get Role Chains
	Declare
		@RoleNodeChain Table (
		[Id] UniqueIdentifier,
		[NodeChain] nvarchar(4000)
	)

	Insert into
		@RoleNodeChain
	Select
		rolN.RoleId, STRING_AGG(Cast(rolN.NodeId as NVARCHAR(36)), '_') WITHIN GROUP ( ORDER BY rolN.[Order] ASC) as 'NodeChain'
	FROM 
		[dbo].[RoleNodes] rolN
	Where
		rolN.RoleId not in (
			Select 
				Id
			From
				[dbo].[Roles] inner join
				(Select Distinct
					TreeId
				From
					[dbo].[TreeRequiredNodes ]
				Where
					NodeId not in (Select [Id] from @MemberNodes)
			) as ExcludeTrees on [dbo].[Roles].TreeId = ExcludeTrees.TreeId
		)
	Group By
		rolN.RoleId

	-- Get Roles
	Select
		Rol.Id
	From
		[dbo].[Roles] rol
	Where
		rol.Id not in (
		Select
			rChain.Id
		From
			@RoleNodeChain rChain inner join
			(Select
				CONCAT(NodeChain, '%') as ChainLike
			From
				@RoleNodeChain chain inner join
				(Select Distinct
					RoleId
				From
					[dbo].[RoleRequiredNodes]
				Where
					NodeId not in (Select [Id] from @MemberNodes)
				) as rEx on chain.Id=rEx.RoleId
			) as rLike on rChain.NodeChain like rLike.ChainLike
		)
END
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberRoles");

            migrationBuilder.DropTable(
                name: "RoleNodes");

            migrationBuilder.DropTable(
                name: "RoleRequiredNodes");

            migrationBuilder.DropTable(
                name: "TreeRequiredNodes");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "Trees");

            migrationBuilder.Sql("DROP PROCEDURE [dbo].[sp_GetAvailableMemberRoles]");
        }
    }
}
