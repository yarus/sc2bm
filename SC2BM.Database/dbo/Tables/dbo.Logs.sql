CREATE TABLE [dbo].[Logs] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [AddedDate]      DATETIME       NOT NULL,
    [OwnerUserID]    INT            NULL,
    [Uri]            NVARCHAR (500) NULL,
    [Message]        NVARCHAR (MAX) NOT NULL,
    [AdditionalInfo] NVARCHAR (MAX) NULL,
    [ServerName]     NVARCHAR (500) NOT NULL,
    [LogType]        NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([ID] ASC)
);





