-- =============================================
-- Author:		AKardash
-- Create date: 10/28/2015
-- Description:	Delete Rate
-- =============================================
CREATE PROCEDURE [dbo].[Rates_Delete]	
	@RateID int 	
AS
BEGIN
	DELETE FROM dbo.Rates
	WHERE ID = @RateID
END