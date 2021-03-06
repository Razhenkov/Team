﻿CREATE TABLE [dbo].[Users] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (25)  NOT NULL,
    [LastName]     NVARCHAR (25)  NOT NULL,
    [EmailAddress] VARCHAR (120)  NOT NULL,
    [Password]     NVARCHAR (100) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  NOT NULL,
    [LastLoginAt]  DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);



