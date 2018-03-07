CREATE PROCEDURE [dbo].[Rates_Insert]
	@EntityType nvarchar(100),
	@EntityID int,
	@Value int,
	@OwnerUserID int,
	@AddedDate datetime
AS
BEGIN
	INSERT INTO [dbo].[Rates]
			   ([EntityType]
			   ,[EntityID]
			   ,[Value]
			   ,[OwnerUserID]
			   ,[AddedDate])
		 VALUES
			   (@EntityType
			   ,@EntityID
			   ,@Value
			   ,@OwnerUserID
			   ,@AddedDate);

	SELECT SCOPE_IDENTITY();
END