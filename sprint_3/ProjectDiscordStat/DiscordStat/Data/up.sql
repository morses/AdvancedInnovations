-- CREATE DATABASE [DiscordData];
-- GO

-- USE [DiscordData];
-- GO

-- *************** Create tables/entities ********************
CREATE TABLE [Server] 
(
  [ID]            nvarchar(128) Not Null,  
  [ServerPk]    int          PRIMARY KEY IDENTITY(1, 1),
  [Name]        nvarchar(256) NULL,
  [Owner]        nvarchar(256) NULL,
  [Icon]        nvarchar(256) Null,
  [HasBot]        nvarchar(256) Null,
  [Approximate_Member_Count] int Null,
  [owner_id] nvarchar(256) Null,
  [verification_level] nvarchar(256) Null,
  [description] nvarchar(256) Null,
  [premium_tier] nvarchar(256) Null,
  [approximate_presence_count] int Null,
  [Privacy] nvarchar(256) Null,
  [OnForum] nvarchar(256) Null,
  [Message] nvarchar(256) Null
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

CREATE TABLE [MessageInfo] 
(
  [MessageDataPk] int           PRIMARY KEY IDENTITY(1, 1),
  [ServerId] nvarchar(256) NULL,
  [ChannelId] nvarchar(256) NULL,
  [UserId] nvarchar(256) NULL,
  [CreatedAt] nvarchar(256) NULL,

);

CREATE TABLE [Presence]
(
  [PresencePk] int PRIMARY KEY IDENTITY(1, 1),
  [ID] nvarchar(256) NULL,
  [applicationID] nvarchar(256) Null,
  [Name]    nvarchar(256)  NULL,
  [Details] nvarchar(256) NULL,
  [CreatedAt] DATETIME NULL,
  [LargeImageId] nvarchar(256) NULL,
  [SmallImageId] nvarchar(256) NULL,
  [ServerId] nvarchar(256) NULL,
  [UserId] NVARCHAR(256) NULL,
  [Image] NVARCHAR(256) NULL
);

CREATE TABLE [ServerPresenceJoin] 
(
  [ID]       int          PRIMARY KEY IDENTITY(1, 1),
  [ServerPk] int,
  [PresencePk]   int
);

CREATE TABLE [Channel]
(
  [ChannelPk] int PRIMARY KEY IDENTITY(1,1),
  [ID] nvarchar(256) Null,
  [Type] nvarchar(256) Null,
  [Name] nvarchar(256) Null,
  [Count] int Null,
  [Guild_id] nvarchar(256) Null,
);

CREATE TABLE [ServerChannelJoin]
(
  [ID] int PRIMARY KEY IDENTITY(1,1),
  [ServerPk] int,
  [ChannelPk]   int
);


CREATE TABLE [Webhook]
(
  [WebhookPk] int PRIMARY KEY IDENTITY(1,1),
  [ID] nvarchar(256) Null,
  [Type] nvarchar(256) Null,
  [Name] nvarchar(256) Null,
  [Avatar]  nvarchar(256) Null,
  [Channel_id] nvarchar(256) Null,
  [Guild_id] nvarchar(256) Null,
  [Application_id] nvarchar(256) Null,
  [Token] nvarchar(256) Null,
);

CREATE TABLE [ChannelWebhookJoin]
(
  [ID] int PRIMARY KEY IDENTITY(1,1),
  [ChannelPk] int,
  [WebhookPk]   int
);


CREATE TABLE [VoiceChannels]
(
  [VoiceChannelPk] int PRIMARY KEY IDENTITY(1,1),
  [ID] nvarchar(256) Null,
  [Name] nvarchar(256) Null,
  [Count] int Null,
  [Guild_id] nvarchar(256) Null,
  [Time] DateTime Null
);


-- *************** Add foreign key relations ********************
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerUserJoinServerPk]        FOREIGN KEY ([ServerPk])        REFERENCES [Server]        ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerUserJoin] ADD CONSTRAINT [ServerUserJoinDiscordUserPk]   FOREIGN KEY ([DiscordUserPk])   REFERENCES [DiscordUser]   ([DiscordUserPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerPresenceJoin] ADD CONSTRAINT [ServerPresenceJoinServerPk]        FOREIGN KEY ([ServerPk])        REFERENCES [Server]        ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerPresenceJoin] ADD CONSTRAINT [ServerPresenceJoinPresencePk]   FOREIGN KEY ([PresencePk])   REFERENCES [Presence]   ([PresencePk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerChannelJoin] ADD CONSTRAINT [ServerChannelJoinServerPk]        FOREIGN KEY ([ServerPk])        REFERENCES [Server]        ([ServerPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ServerChannelJoin] ADD CONSTRAINT [ServerChannelJoinChannelPk]   FOREIGN KEY ([ChannelPk])   REFERENCES [Channel]   ([ChannelPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ChannelWebhookJoin] ADD CONSTRAINT [ChannelWebhookJoinChannelPk]        FOREIGN KEY ([ChannelPk])        REFERENCES [Channel]        ([ChannelPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [ChannelWebhookJoin] ADD CONSTRAINT [ChannelWebhookJoinWebhookPk]   FOREIGN KEY ([WebhookPk])   REFERENCES [Webhook]   ([WebhookPk]) ON DELETE NO ACTION ON UPDATE NO ACTION;