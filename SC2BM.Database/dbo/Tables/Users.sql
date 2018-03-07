CREATE TABLE [dbo].[Users] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [UserName]       NVARCHAR (50)  NOT NULL,
    [FirstName]      NVARCHAR (100) NOT NULL,
    [LastName]       NVARCHAR (255) NOT NULL,
    [BirthDate]      DATE           NOT NULL,
    [Email]          NVARCHAR (255) NOT NULL,
    [Password]       NVARCHAR (255) NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [ActivationSalt] NVARCHAR (255) NOT NULL,
    [RegisteredDate] DATE           NOT NULL,
    [Roles]          NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([ID] ASC)
);



