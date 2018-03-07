CREATE TABLE [dbo].[BlogPosts] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [OwnerUserID] INT            NOT NULL,
    [BlogID]      INT            NOT NULL,
    [LogoPath]    NVARCHAR (255) NULL,
    [Title]       NVARCHAR (255) NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    CONSTRAINT [PK_BlogPosts] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BlogPosts_Blogs] FOREIGN KEY ([BlogID]) REFERENCES [dbo].[Blogs] ([ID]),
    CONSTRAINT [FK_BlogPosts_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);



