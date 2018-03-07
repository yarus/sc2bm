-- =============================================
-- Author:		AKardash
-- Create date: 11/11/2015
-- Description:	Add new row into Blogs table
-- =============================================
CREATE PROCEDURE [dbo].[Blogs_Insert]
	@Title nvarchar(255),
	@Description nvarchar(1000),
	@OwnerUserID int,
	@AddedDate datetime,
	@LogoPath nvarchar(255),
	@IsDeleted bit
AS
BEGIN
	INSERT INTO [dbo].[Blogs]
			   ([Title]
			   ,[Description]
			   ,[OwnerUserID]
			   ,[AddedDate]
			   ,[LogoPath]
			   ,[IsDeleted])
		 VALUES
			   (@Title, @Description, @OwnerUserID, @AddedDate, @LogoPath, @IsDeleted);

	SELECT SCOPE_IDENTITY();
END