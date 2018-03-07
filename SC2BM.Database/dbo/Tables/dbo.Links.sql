CREATE TABLE [dbo].[Links] (
    [ID]            INT             IDENTITY (1, 1) NOT NULL,
    [Type]          NVARCHAR (100)  NOT NULL,
    [MainLink]      NVARCHAR (1000) NOT NULL,
    [SecondaryLink] NVARCHAR (1000) NULL,
    [DisplayText]   NVARCHAR (255)  NOT NULL,
    [EntityType]    NVARCHAR (100)  NOT NULL,
    [EntityID]      INT             NOT NULL,
    [OwnerUserID]   INT             NOT NULL,
    [AddedDate]     DATETIME        NOT NULL,
    CONSTRAINT [PK_Links] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Links_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);

