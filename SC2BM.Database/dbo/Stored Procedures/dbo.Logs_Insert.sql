CREATE PROCEDURE [dbo].[Logs_Insert]
    @OwnerUserID int,
	@Uri nvarchar(500),
	@Message nvarchar(max),
    @AdditionalInfo nvarchar(max),
    @ServerName nvarchar(500),
    @LogType nvarchar(100)
AS
BEGIN

INSERT INTO [dbo].[Logs]
           ([AddedDate]
           ,[OwnerUserID]
           ,[Uri]
           ,[Message]
           ,[AdditionalInfo]
           ,[ServerName]
           ,[LogType])
     VALUES
           (getdate()
           ,@OwnerUserID
           ,@Uri
           ,@Message
           ,@AdditionalInfo
           ,@ServerName
           ,@LogType)

	SELECT SCOPE_IDENTITY();
END