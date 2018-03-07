CREATE PROCEDURE [dbo].[Rates_Update]
	@RateID int,
	@EntityType nvarchar(100),
	@EntityID int,
	@Value int,
	@OwnerUserID int,
	@AddedDate datetime	
AS
BEGIN
	UPDATE [dbo].[Rates]
	SET EntityType = @EntityType
		,EntityID = @EntityID
		,Value = @Value
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate		
	WHERE ID = @RateID;
END