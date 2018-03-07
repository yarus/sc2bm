CREATE PROCEDURE [dbo].[Links_Delete]	
	@LinkID int 	
AS
BEGIN
	DELETE FROM dbo.Links
	WHERE ID = @LinkID
END