CREATE TABLE [dbo].[Blogs] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [OwnerUserID] INT             NOT NULL,
    [Title]       NVARCHAR (255)  NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    [IsDeleted]   BIT             NOT NULL,
    [AddedDate]   DATETIME        NOT NULL,
    [LogoPath]    NVARCHAR (255)  NULL,
    CONSTRAINT [PK_Blogs] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Blogs_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);



