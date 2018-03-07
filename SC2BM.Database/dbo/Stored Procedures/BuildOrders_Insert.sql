CREATE PROCEDURE [dbo].[BuildOrders_Insert]
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
	INSERT INTO [dbo].[BuildOrders]
			   ([Name]
			   ,[SC2VersionID]
			   ,[Description]
			   ,[Race]
			   ,[VsRace]
			   ,[BuildItems]
			   ,[OwnerUserID]
			   ,[AddedDate]
			   ,[IsDeleted]
			   ,[MicroRate]
			   ,[MacroRate]
			   ,[AggressionRate]
			   ,[DefenceRate])
		 VALUES
			   (@Name
			   ,@SC2VersionID
			   ,@Description
			   ,@Race
			   ,@VsRace
			   ,@BuildItems
			   ,@OwnerUserID
			   ,@AddedDate
			   ,@IsDeleted
			   ,@Micro
			   ,@Macro
			   ,@Aggression
			   ,@Defence);

	SELECT SCOPE_IDENTITY();
END