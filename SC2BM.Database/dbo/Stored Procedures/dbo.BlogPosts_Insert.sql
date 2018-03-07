-- =============================================
-- Author:		AKardash
-- Create date: 11/12/2015
-- Description:	Add new row into BlogPosts table
-- =============================================
CREATE PROCEDURE [dbo].[BlogPosts_Insert]
	@Title nvarchar(255),
	@Text nvarchar(max),
	@BlogID int,
	@OwnerUserID int,
	@AddedDate datetime,
	@LogoPath nvarchar(255),
	@IsDeleted bit
AS
BEGIN
	INSERT INTO [dbo].[BlogPosts]
			   ([Title]
			   ,[Text]
			   ,[BlogID]
			   ,[OwnerUserID]
			   ,[AddedDate]
			   ,[LogoPath]
			   ,[IsDeleted])
		 VALUES
			   (@Title, @Text, @BlogID, @OwnerUserID, @AddedDate, @LogoPath, @IsDeleted);

	SELECT SCOPE_IDENTITY();
END