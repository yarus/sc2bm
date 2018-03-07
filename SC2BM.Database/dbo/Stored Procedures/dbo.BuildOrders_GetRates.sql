-- =============================================
-- Author:		AKardash
-- Create date: 12/10/2015
-- Description:	Get Rates for Build Orders
-- =============================================
CREATE PROCEDURE [dbo].[BuildOrders_GetRates]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT bp.ID AS ID, 'Build' As Type, CAST(AVG(r.Value*1.0) AS DECIMAL(18,2)) As Value
	FROM [dbo].BuildOrders bp, [dbo].Rates r
	WHERE r.EntityType='Build' AND r.EntityID = bp.[ID]
	Group By bp.ID
	ORDER BY Value DESC
END