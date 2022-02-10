-- CREATE DATABASE [DiscordData];
-- GO

-- USE [DiscordData];
-- GO

-- *************** Create tables/entities ********************
CREATE TABLE [Server] 
(
  [ID]    int          PRIMARY KEY IDENTITY(1, 1),
  [Name]  nvarchar(50) NOT NULL,
  [Owner] nvarchar(50) NOT NULL
);

CREATE TABLE [ServerUserJoin] 
(
  [ID]       int          PRIMARY KEY IDENTITY(1, 1),
  [ServerID] int,
  [UserID]   int
);

CREATE TABLE [User] 
(
  [ID]      int           PRIMARY KEY IDENTITY(1, 1),
  [Name]    nvarchar(50)  NOT NULL,
  [Servers] nvarchar(256) NOT NULL
);

-- *************** Add foreign key relations ********************
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerID] FOREIGN KEY ([ServerID]) REFERENCES [Server] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [UserID]   FOREIGN KEY ([UserID])   REFERENCES [User]   ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
