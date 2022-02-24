ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [ServerPk];
ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [DiscordUserID];

DROP TABLE [Server];
DROP TABLE [ServerUserJoin];
DROP TABLE [DiscordUser];

