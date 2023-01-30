-- Omited procedure - functionality replaced kept for refrence
-- Get available roles that requirements have been met for
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