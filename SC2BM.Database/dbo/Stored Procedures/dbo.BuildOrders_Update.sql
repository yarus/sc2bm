CREATE PROCEDURE [dbo].[BuildOrders_Update]
	@BuildOrderID int,
	@Name nvarchar(255),
	@SC2VersionID nvarchar(50),
	@Description nvarchar(max),
	@Race nvarchar(10),
	@VsRace nvarchar(10),
	@BuildItems nvarchar(max),
	@OwnerUserID int,
	@AddedDate datetime,
	@IsDeleted bit,
	@Micro int,
	@Macro int,
	@Aggression int,
	@Defence int
AS
BEGIN
	UPDATE [dbo].[BuildOrders]
	SET Name = @Name
		,SC2VersionID = @SC2VersionID
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate
		,Description = @Description
		,Race = @Race
		,VsRace = @VsRace
		,BuildItems = @BuildItems
		,IsDeleted = @IsDeleted
		,MicroRate = @Micro
		,MacroRate = @Macro
		,AggressionRate = @Aggression
		,DefenceRate = @Defence
	WHERE ID = @BuildOrderID;
END