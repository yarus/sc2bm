CREATE TABLE [dbo].[BuildOrders] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (255) NOT NULL,
    [SC2VersionID]   NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [Race]           NVARCHAR (10)  NOT NULL,
    [VsRace]         NVARCHAR (10)  NOT NULL,
    [BuildItems]     NVARCHAR (MAX) NOT NULL,
    [OwnerUserID]    INT            NOT NULL,
    [AddedDate]      DATETIME       NOT NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_BuildOrders_IsDeleted] DEFAULT ((0)) NOT NULL,
    [MicroRate]      INT            CONSTRAINT [DF_BuildOrders_MicroRate] DEFAULT ((0)) NULL,
    [MacroRate]      INT            CONSTRAINT [DF_BuildOrders_MacroRate] DEFAULT ((0)) NULL,
    [AggressionRate] INT            CONSTRAINT [DF_BuildOrders_AggressionRate] DEFAULT ((0)) NULL,
    [DefenceRate]    INT            CONSTRAINT [DF_BuildOrders_DefenceRate] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_BuildOrders] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BuildOrders_Users] FOREIGN KEY ([OwnerUserID]) REFERENCES [dbo].[Users] ([ID])
);











