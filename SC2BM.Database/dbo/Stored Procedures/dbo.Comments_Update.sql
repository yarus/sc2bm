CREATE PROCEDURE [dbo].[Comments_Update]
	@CommentID int,
	@EntityType nvarchar(25),
	@EntityID int,
	@Text nvarchar(max),
	@OwnerUserID int,
	@AddedDate datetime,
	@IsDeleted bit
AS
BEGIN
	UPDATE [dbo].[Comments]
	SET EntityType = @EntityType
		,EntityID = @EntityID
		,Text = @Text
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate
		,IsDeleted = @IsDeleted
	WHERE ID = @CommentID;
END