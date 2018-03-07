-- =============================================
-- Author:		AKardash
-- Create date: 11/18/2015
-- Description:	Get Rates for Blogs
-- =============================================
CREATE PROCEDURE [dbo].[Blogs_GetRates]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT bp.BlogID AS ID, 'Blog' AS Type, CAST(AVG(r.Value*1.0) AS DECIMAL(18,2)) As Value
	FROM [dbo].BlogPosts bp, [dbo].Rates r
	WHERE r.EntityType='BlogPost' AND r.EntityID = bp.[ID]
	Group By bp.BlogID
	ORDER BY Value DESC
END