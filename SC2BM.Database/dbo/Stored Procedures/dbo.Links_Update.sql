CREATE PROCEDURE [dbo].[Links_Update]
	@LinkID int,
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
	UPDATE [dbo].[Links]
	SET		
		EntityType = @EntityType
		,EntityID = @EntityID
		,DisplayText = @DisplayText
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate
		,MainLink = @MainLink
		,SecondaryLink = @SecondaryLink
		,[Type] = @Type
	WHERE ID = @LinkID;
END