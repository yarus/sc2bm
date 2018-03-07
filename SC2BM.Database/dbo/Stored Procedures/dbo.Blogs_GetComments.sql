-- =============================================
-- Author:		AKardash
-- Create date: 11/18/2015
-- Description:	Get Latest Comments for Blog
-- =============================================
CREATE PROCEDURE [dbo].[Blogs_GetComments]
	@BlogID As INTEGER
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT c.*, u.UserName As OwnerUserName
	FROM [dbo].BlogPosts bp, [dbo].Comments c, [dbo].Users u
	WHERE bp.ID = c.EntityID AND c.EntityType='BlogPost' AND bp.BlogID=@BlogID AND c.OwnerUserID = u.ID
	ORDER BY AddedDate DESC
END