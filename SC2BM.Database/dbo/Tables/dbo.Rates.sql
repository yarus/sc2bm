CREATE TABLE [dbo].[Rates] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [EntityType]  NVARCHAR (100) NOT NULL,
    [EntityID]    INT            NOT NULL,
    [Value]       INT            NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [OwnerUserID] INT            NOT NULL,
    CONSTRAINT [PK_Rates] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Rates_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);

