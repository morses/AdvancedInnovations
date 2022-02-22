-- CREATE DATABASE [DiscordData];
-- GO

-- USE [DiscordData];
-- GO

-- *************** Create tables/entities ********************
CREATE TABLE [Server] 
(
  [ID]			nvarchar(128) Not Null,  
  [ServerPk]	int          PRIMARY KEY IDENTITY(1, 1),
  [Name]		nvarchar(50) NOT NULL,
  [Owner]		nvarchar(50) NOT NULL,
  [Icon]		nvarchar(256) Null,
  [HasBot]		nvarchar(50) Not Null,
  [ApproximateMemberCount] int Null
);

CREATE TABLE [ServerUserJoin] 
(
  [ID]       int          PRIMARY KEY IDENTITY(1, 1),
  [ServerPk] int,
  [DiscordUserID]   int
);

CREATE TABLE [DiscordUser] 
(
  [ID]      int           PRIMARY KEY IDENTITY(1, 1),
  [Name]    nvarchar(50)  NOT NULL,
  [Servers] nvarchar(256) NOT NULL,
  [Avatar] nvarchar(50)	 NULL
);

-- *************** Add foreign key relations ********************
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerPk] FOREIGN KEY ([ServerPk]) REFERENCES [Server] ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [DiscordUserID]   FOREIGN KEY ([DiscordUserID])   REFERENCES [DiscordUser]   ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
