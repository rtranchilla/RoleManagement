using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleManager.DataPersistence.Migrations
{
    public partial class RoleNameFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE or Alter FUNCTION dbo.RoleIdFromName
(
	@Name nvarchar(450),
	@Tree nvarchar(450)
)
RETURNS uniqueidentifier
AS
BEGIN
	DECLARE @ResultVar uniqueidentifier

	Select Top 1
		@ResultVar = [Id]
	From (
		SELECT
			rol.[Id],STRING_AGG(nod.[Name], '_') WITHIN GROUP ( ORDER BY rolN.[Order] ASC) as 'Name'
		FROM 
			[dbo].[Roles] rol inner join
			[dbo].[RoleNodes] rolN on rol.Id = rolN.RoleId inner join
			[dbo].[Nodes] nod on nod.Id = rolN.NodeId
		Where
			rol.TreeId = (Select Top 1 [Id] from [dbo].[Trees] where Name = @Tree)
		Group By
			rol.Id) as RoleNames
	Where
		Name = @Name

	RETURN @ResultVar
END
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP FUNCTION dbo.RoleIdFromName ");
        }
    }
}
