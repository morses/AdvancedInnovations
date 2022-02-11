using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;

namespace DiscordStats_Tests
{
    public class DiscordService_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetGuilds_ValidDataForTwoServersFromDiscord_ShouldParseOK()
        {
            // Arrange
            DiscordService discord = new DiscordService();
            // I didn't know what this was supposed to look like so I copied what I thought
            // was the correct example from the Discord API docs.  Doesn't look right though as
            // owner here is a bool but yours is a string.
            string jsonFromDiscordAPI = @"[{
  ""id"": 1035111022,
  ""name"": ""1337 Krew"",
  ""icon"": ""8342729096ea3675442027381ff50dfe"",
  ""owner"": ""true"",
  ""permissions"": ""36953089"",
  ""features"": [""COMMUNITY"", ""NEWS""]
},
{
  ""id"": 1345452453,
  ""name"": ""Omicron Knew"",
  ""icon"": ""8872722096ea3634442027381ff50dbc"",
  ""owner"": ""true"",
  ""permissions"": ""36953089"",
  ""features"": [""GAMES"", ""IMAGES""]
}]";
            // Act
            List<Server>? servers = discord.GetCurrentUserGuilds("fakeBearerToken", (bt, uri) => jsonFromDiscordAPI);

            // Assert
            Assert.Multiple(() =>
                {
                    Assert.That(servers!.Count, Is.EqualTo(2));
                    Assert.That(servers[0].Id == 1035111022);
                    Assert.That(servers[0].Name == "1337 Krew");
                    Assert.That(servers[0].Owner == "true");
                }
            );
        }

        [Test]
        public void GetGuilds_EmptyStringFromDiscord_ShouldThrowException()
        {
            Assert.Fail();
        }

        // And continue with tests
    }
}