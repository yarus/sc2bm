-- =============================================
-- Author:		AKardash
-- Create date: 10/22/2015
-- Description:	Add new row into News table
-- =============================================
CREATE PROCEDURE [dbo].[News_Insert]
	@Title nvarchar(150),
	@Text nvarchar(max),
	@OwnerUserID int,
	@AddedDate datetime,
	@LogoPath nvarchar(255),
	@IsDeleted bit
AS
BEGIN
	INSERT INTO [dbo].[News]
			   ([Title]
			   ,[Text]
			   ,[OwnerUserID]
			   ,[AddedDate]
			   ,[LogoPath]
			   ,[IsDeleted])
		 VALUES
			   (@Title, @Text, @OwnerUserID, @AddedDate, @LogoPath, @IsDeleted);

	SELECT SCOPE_IDENTITY();
END