-- =============================================
-- Author:		AKardash
-- Create date: 11/11/2015
-- Description:	Update existing row in Blogs table
-- =============================================
CREATE PROCEDURE [dbo].[Blogs_Update]
	@BlogID int,
	@Title nvarchar(255),
	@Description nvarchar(1000),
	@OwnerUserID int,
	@AddedDate datetime,
	@LogoPath nvarchar(255),
	@IsDeleted bit
AS
BEGIN
	UPDATE [dbo].[Blogs]
	SET Title = @Title
		,[Description] = @Description
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate
		,LogoPath = @LogoPath
		,IsDeleted = @IsDeleted
	WHERE ID = @BlogID;
END