-- =============================================
-- Author:		AKardash
-- Create date: 11/12/2015
-- Description:	Update existing row in BlogPosts table
-- =============================================
CREATE PROCEDURE [dbo].[BlogPosts_Update]
	@BlogPostID int,
	@Title nvarchar(150),
	@Text nvarchar(max),
	@BlogID int,
	@OwnerUserID int,
	@AddedDate datetime,
	@LogoPath nvarchar(255),
	@IsDeleted bit
AS
BEGIN
	UPDATE [dbo].[BlogPosts]
	SET Title = @Title
		,Text = @Text
		,BlogID = @BlogID
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate
		,LogoPath = @LogoPath
		,IsDeleted = @IsDeleted
	WHERE ID = @BlogPostID;
END