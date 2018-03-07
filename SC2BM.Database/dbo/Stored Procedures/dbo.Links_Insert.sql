CREATE PROCEDURE [dbo].[Links_Insert]
	@EntityType nvarchar(25),
	@EntityID int,	
	@OwnerUserID int,
	@AddedDate datetime,
	@Type nvarchar(100),
	@MainLink nvarchar(1000),
	@SecondaryLink nvarchar(1000),
	@DisplayText nvarchar(255)
AS
BEGIN
	INSERT INTO [dbo].[Links]
			   ([Type]
			   ,[MainLink]
			   ,[SecondaryLink]
			   ,[DisplayText]
			   ,[EntityType]
			   ,[EntityID]			   
			   ,[OwnerUserID]
			   ,[AddedDate])
		 VALUES
			   (@Type
			   ,@MainLink
			   ,@SecondaryLink
			   ,@DisplayText
			   ,@EntityType
			   ,@EntityID			   
			   ,@OwnerUserID
			   ,@AddedDate);

	SELECT SCOPE_IDENTITY();
END