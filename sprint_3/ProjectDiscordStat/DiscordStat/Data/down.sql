 ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [ServerUserJoinServerPk]
 ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [ServerUserJoinDiscordUserPk]
 ALTER TABLE [ServerPresenceJoin] DROP CONSTRAINT [ServerPresenceJoinServerPk]
 ALTER TABLE [ServerPresenceJoin] DROP CONSTRAINT [ServerPresenceJoinPresencePk]
 ALTER TABLE [ServerChannelJoin] DROP CONSTRAINT [ServerChannelJoinServerPk]
 ALTER TABLE [ServerChannelJoin] DROP CONSTRAINT [ServerChannelJoinChannelPk]
 ALTER TABLE [ChannelWebhookJoin] DROP CONSTRAINT [ChannelWebhookJoinChannelPk]
 ALTER TABLE [ChannelWebhookJoin] DROP CONSTRAINT [ChannelWebhookJoinWebhookPk]

 DROP TABLE [Server];
 DROP TABLE [ServerUserJoin];
 DROP TABLE [DiscordUser];
 DROP TABLE [MessageInfo]
 DROP TABLE [Presence];
 DROP TABLE [ServerPresenceJoin];
 DROP TABLE [Channel];
 DROP TABLE [ServerChannelJoin];
 DROP TABLE [Webhook];
 DROP TABLE [ChannelWebhookJoin];


