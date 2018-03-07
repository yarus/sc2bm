-- =============================================
-- Author:		AKardash
-- Create date: 10/23/2015
-- Description:	Update existing row in News table
-- =============================================
CREATE PROCEDURE [dbo].[News_Update]
	@NewsItemID int,
	@Title nvarchar(150),
	@Text nvarchar(max),
	@OwnerUserID int,
	@AddedDate datetime,
	@LogoPath nvarchar(255),
	@IsDeleted bit
AS
BEGIN
	UPDATE [dbo].[News]
	SET Title = @Title
		,Text = @Text
		,OwnerUserID = @OwnerUserID
		,AddedDate = @AddedDate
		,LogoPath = @LogoPath
		,IsDeleted = @IsDeleted
	WHERE ID = @NewsItemID;
END