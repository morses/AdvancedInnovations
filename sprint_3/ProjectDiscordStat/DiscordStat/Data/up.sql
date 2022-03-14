CREATE DATABASE [DiscordData];
GO

USE [DiscordData];
GO

-- *************** Create tables/entities ********************
CREATE TABLE [Server] 
(
  [ID]            nvarchar(128) Not Null,  
  [ServerPk]    int          PRIMARY KEY IDENTITY(1, 1),
  [Name]        nvarchar(50) NOT NULL,
  [Owner]        nvarchar(50) NOT NULL,
  [Icon]        nvarchar(256) Null,
  [HasBot]        nvarchar(50) Not Null,
  [Approximate_Member_Count] int Null,
  [owner_id] nvarchar(50) Not Null,
  [verification_level] nvarchar(50) Not Null,
  [description] nvarchar(256) Not Null,
  [premium_tier] nvarchar(50) Not Null,
  [approximate_presence_count]nvarchar(50) Not Null,
  [Privacy] nvarchar(50) Not Null,
  [OnForum] nvarchar(50) Not Null,
  [Message] nvarchar(50) Not Null
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
  [Username]    nvarchar(128)  NOT NULL,
  [Servers] nvarchar(256) NOT NULL,
  [Avatar] nvarchar(256)     NULL
);

CREATE TABLE [Presence]
(
  [PresencePk] int PRIMARY KEY IDENTITY(1, 1),
  [ID] nvarchar(256) NULL,
  [applicationID] nvarchar(256) Null,
  [Name]    nvarchar(256)  NULL,
  [Details] nvarchar(256) NULL,
  [CreatedAt] nvarchar(256) NULL,
  [LargeImageId] nvarchar(256) NULL,
  [SmallImageId] nvarchar(256) NULL,
  [ServerId] nvarchar(256) NULL,
  [UserId] NVARCHAR(256) NULL
);

CREATE TABLE [ServerPresenceJoin] 
(
  [ID]       int          PRIMARY KEY IDENTITY(1, 1),
  [ServerPk] int,
  [PresencePk]   int
);

CREATE TABLE [Channel]
(
  [ChannelsPk] int PRIMARY KEY IDENTITY(1,1),
  [ID] nvarchar(256) Not Null,
  [Type] int Not Null,
  [Name] nvarchar(256) Not Null,
  [Position] int Not Null,
  [Parent_id] nvarchar(256) Not Null,
  [Guild_id] nvarchar(256) Not Null,
  [Permission_overwrites] nvarchar(256) Null,
  [Nsfw] nvarchar(50) Null
)

CREATE TABLE [ServerChannelJoin]
(
  [ID] int PRIMARY KEY IDENTITY(1,1),
  [ServerPk] int,
  [ChannelsPk]   int
)


-- *************** Add foreign key relations ********************
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerUserJoinServerPk]        FOREIGN KEY ([ServerPk])        REFERENCES [Server]        ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerUserJoinDiscordUserPk]   FOREIGN KEY ([DiscordUserPk])   REFERENCES [DiscordUser]   ([DiscordUserPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerPresenceJoin] ADD CONSTRAINT [ServerPresenceJoinServerPk]        FOREIGN KEY ([ServerPk])        REFERENCES [Server]        ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerPresenceJoin] ADD CONSTRAINT [ServerPresenceJoinPresencePk]   FOREIGN KEY ([PresencePk])   REFERENCES [Presence]   ([PresencePk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerChannelJoin] ADD CONSTRAINT [ServerChannelJoinServerPk]        FOREIGN KEY ([ServerPk])        REFERENCES [Server]        ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerChannelJoin] ADD CONSTRAINT [ServerChannelJoinChannelsPk]   FOREIGN KEY ([ChannelPk])   REFERENCES [Channel]   ([ChannelPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;