CREATE TABLE [dbo].[News] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (150) NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    [OwnerUserID] INT            NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [LogoPath]    NVARCHAR (255) NOT NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_News_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_News_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);





