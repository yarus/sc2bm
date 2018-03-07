-- =============================================
-- Author:		AKardash
-- Create date: 12/09/2015
-- Description:	Get Rates for Blog pOSTS
-- =============================================
CREATE PROCEDURE [dbo].[BlogPosts_GetRates]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT bp.ID AS ID, 'BlogPost' As Type, CAST(AVG(r.Value*1.0) AS DECIMAL(18,2)) As Value
	FROM [dbo].BlogPosts bp, [dbo].Rates r
	WHERE r.EntityType='BlogPost' AND r.EntityID = bp.[ID]
	Group By bp.ID
	ORDER BY Value DESC
END