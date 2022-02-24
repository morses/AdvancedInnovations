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
  [Approximate_Member_Count] int Null
);

CREATE TABLE [ServerUserJoin] 
(
  [ID]       int          PRIMARY KEY IDENTITY(1, 1),
  [ServerPk] int,
  [DiscordUserPk]   int
);

CREATE TABLE [DiscordUser] 
(
  [ID]      nvarchar(128) Not Null, 
  [DiscordUserPk] int           PRIMARY KEY IDENTITY(1, 1),
  [Name]    nvarchar(50)  NOT NULL,
  [Servers] nvarchar(256) NOT NULL,
  [Avatar] nvarchar(256)	 NULL
);

-- *************** Add foreign key relations ********************
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerPk] FOREIGN KEY ([ServerPk]) REFERENCES [Server] ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [DiscordUserPk]   FOREIGN KEY ([DiscordUserPk])   REFERENCES [DiscordUser]   ([DiscordUserPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
