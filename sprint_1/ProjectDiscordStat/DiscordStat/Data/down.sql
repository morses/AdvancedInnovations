ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [ServerPk];
ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [DiscordUserPk];

DROP TABLE [Server];
DROP TABLE [ServerUserJoin];
DROP TABLE [DiscordUser];

