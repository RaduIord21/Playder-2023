CREATE TABLE [dbo].[Competition] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [name]          VARCHAR (255) NULL,
    [NumberOfTeams] INT           NULL,
    [Adress]        VARCHAR (255) NULL,
    [StartDate]     DATE          NULL,
    [EndDate]       DATE          NULL,
    [SponsorId]     INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([SponsorId]) REFERENCES [dbo].[Sponsor] ([id])
);


GO

CREATE TABLE [dbo].[Player] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [FullName]    VARCHAR (255) NULL,
    [BirthDate]   DATE          NULL,
    [Adress]      VARCHAR (255) NULL,
    [Position]    INT           NULL,
    [TeamId]      INT           NULL,
    [ShirtNumber] INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([Id])
);


GO

CREATE TABLE [dbo].[TeamSponsor] (
    [id]        INT IDENTITY (1, 1) NOT NULL,
    [TeamId]    INT NULL,
    [SponsorId] INT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([SponsorId]) REFERENCES [dbo].[Sponsor] ([id]),
    FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([Id])
);


GO

CREATE TABLE [dbo].[Coach] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE TABLE [dbo].[CompetitonTeam] (
    [id]            INT IDENTITY (1, 1) NOT NULL,
    [CompetitionId] INT NULL,
    [TeamId]        INT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([CompetitionId]) REFERENCES [dbo].[Competition] ([id]),
    FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([Id])
);


GO

CREATE TABLE [dbo].[Sponsor] (
    [id]   INT           IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


GO

CREATE TABLE [dbo].[Team] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [NickName] VARCHAR (255) NULL,
    [Name]     VARCHAR (255) NULL,
    [CoachId]  INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CoachId]) REFERENCES [dbo].[Coach] ([Id])
);


GO

