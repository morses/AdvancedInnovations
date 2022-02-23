<<<<<<< HEAD
﻿//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net;
//using System.Threading.Tasks;
//using DiscordStats.DAL.Concrete;
//using DiscordStats.Models;
//using Moq;
//using Moq.Contrib.HttpClient;

//namespace DiscordStats_Tests
//{
//    public class DiscordStats_Tests
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public async Task GetGuilds_404Response_ShouldThrowException()
//        {
//            // Arrange  (wrap this up in a method to reuse it)
//            var handler = new Mock<HttpMessageHandler>();

//            handler.SetupAnyRequest()
//                    .ReturnsResponse(HttpStatusCode.NotFound);

//            DiscordService discord = new DiscordService(handler.CreateClientFactory());

//            // Act
//            Task<List<Server>?> Act() => discord.GetCurrentUserGuilds("fakeBearerToken");

//            // Assert
//            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
//        }

//        [Test]
//        public async Task GetGuilds_NotAuthorizedResponse_ShouldThrowException()
//        {
//            // Arrange  (wrap this up in a method to reuse it)
//            var handler = new Mock<HttpMessageHandler>();

//            handler.SetupAnyRequest()
//                    .ReturnsResponse(HttpStatusCode.Unauthorized);

//            DiscordService discord = new DiscordService(handler.CreateClientFactory());

//            // Act
//            Task<List<Server>?> Act() => discord.GetCurrentUserGuilds("fakeBearerToken");

//            // Assert
//            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
//        }

//        [Test]
//        public async Task GetGuilds_ValidDataForTwoServersFromDiscord_ShouldParseOK()
//        {
//            // Arrange  (wrap this up in a method to reuse it)
//            var handler = new Mock<HttpMessageHandler>();

//            string jsonFromDiscordAPI = @"[{
//            ""id"": ""1035111022"",
//            ""name"": ""1337 Krew"",
//            ""icon"": ""8342729096ea3675442027381ff50dfe"",
//            ""owner"": ""true""
//            //""permissions"": ""36953089"",
//            //""features"": [""COMMUNITY"", ""NEWS""]
//            },
//            {
//                ""id"": ""1345452453"",
//                ""name"": ""Omicron Knew"",
//                ""icon"": ""8872722096ea3634442027381ff50dbc"",
//                ""owner"": ""true""
//                //""permissions"": ""36953089"",
//                //""features"": [""GAMES"", ""IMAGES""]
//            }]";
                
//            var response = new HttpResponseMessage()
//            {
//                Content = new StringContent(jsonFromDiscordAPI)
//            };
//            handler.SetupAnyRequest()
//                    .ReturnsAsync(response);

//            DiscordService discord = new DiscordService(handler.CreateClientFactory());

//            // Act
//            List<Server>? servers = await discord.GetCurrentUserGuilds("fakeBearerToken");

//            // Assert
//            Assert.Multiple(() =>
//            {
//                Assert.That(servers!.Count, Is.EqualTo(2));
//                Assert.That(servers[0].Id == "1035111022");
//                Assert.That(servers[0].Name == "1337 Krew");
//                Assert.That(servers[0].Owner == "true");
//            }
//            );

//        }

//        //[Test]
//        //public void GetGuilds_EmptyStringFromDiscord_ShouldThrowException()
//        //{
//        //    Assert.Fail();
//        //}

//        // And continue with tests
//    }
//}
=======
﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;

namespace DiscordStats_Tests
{
    public class DiscordStats_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetGuilds_404Response_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory());

            // Act
            Task<List<Server>?> Act() => discord.GetCurrentUserGuilds("fakeBearerToken");

            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task GetGuilds_NotAuthorizedResponse_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.Unauthorized);

            DiscordService discord = new DiscordService(handler.CreateClientFactory());


            // Act
            Task<List<Server>?> Act() => discord.GetCurrentUserGuilds("fakeBearerToken");

            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task GetGuilds_ValidDataForTwoServersFromDiscord_ShouldParseOK()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"[{
            ""id"": ""1035111022"",
            ""name"": ""1337 Krew"",
            ""icon"": ""8342729096ea3675442027381ff50dfe"",
            ""owner"": ""true""
            //""permissions"": ""36953089"",
            //""features"": [""COMMUNITY"", ""NEWS""]
            },
            {
                ""id"": ""1345452453"",
                ""name"": ""Omicron Knew"",
                ""icon"": ""8872722096ea3634442027381ff50dbc"",
                ""owner"": ""true""
                //""permissions"": ""36953089"",
                //""features"": [""GAMES"", ""IMAGES""]
            }]";
                
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordService discord = new DiscordService(handler.CreateClientFactory());

            // Act
            List<Server>? servers = await discord.GetCurrentUserGuilds("fakeBearerToken");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(servers!.Count, Is.EqualTo(2));
                Assert.That(servers[0].Id == "1035111022");
                Assert.That(servers[0].Name == "1337 Krew");
                Assert.That(servers[0].Owner == "true");
            }
            );

        }

        [Test] public void GetGuildsInfo_WhereBotIsNotInServer__ShouldReturnFalse()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory());
           
            Assert.AreEqual(discord.CheckForBot("FakeBotToken", "FakeServerId").Result, "false");
        }
        
    }
}
>>>>>>> dev
