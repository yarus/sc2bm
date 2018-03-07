CREATE TABLE [dbo].[Comments] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [EntityType]  NVARCHAR (25)  NOT NULL,
    [OwnerUserID] INT            NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [EntityID]    INT            NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Comments_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);



