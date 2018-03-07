CREATE PROCEDURE [dbo].[Comments_Insert]
	@EntityType nvarchar(25),
	@EntityID int,
	@Text nvarchar(max),
	@OwnerUserID int,
	@AddedDate datetime,
	@IsDeleted bit
AS
BEGIN
	INSERT INTO [dbo].[Comments]
			   ([EntityType]
			   ,[EntityID]
			   ,[Text]
			   ,[OwnerUserID]
			   ,[AddedDate]
			   ,[IsDeleted])
		 VALUES
			   (@EntityType
			   ,@EntityID
			   ,@Text
			   ,@OwnerUserID
			   ,@AddedDate
			   ,@IsDeleted);

	SELECT SCOPE_IDENTITY();
END