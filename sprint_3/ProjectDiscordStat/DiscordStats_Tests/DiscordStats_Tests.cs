using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;

namespace DiscordStats_Tests
{
    public class DiscordStats_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private List<Server> _servers = FakeData.Servers;

        private IServerRepository _serverRepository;
        private IPresenceRepository _presenceRepository;

        private Mock<DbSet<ServerPartial>> _mockPartialServerDbSet;    
        //private List<ServerPartial> _serverPartial = FakeData.ServersfromPartial;

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }

        [SetUp]
        public void Setup()
        {
            var ser = new List<Server>
        {
            new Server{Id = "789317480325316640", ServerPk = 1, Name = "input/output server", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=5},
            new Server{Id = "928010025958510632", ServerPk = 2, Name = "Advanced Innovations", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true", ApproximateMemberCount=5},
            new Server{Id = "151516415641361", ServerPk = 3, Name = "fake server", Owner = "false", Icon = "", HasBot = "false", ApproximateMemberCount=23532}
        };

        _mockServerDbSet = GetMockDbSet<Server>(ser.AsQueryable<Server>());
            //_mockPartialServerDbSet = GetMockDbSet(_serverPartial.AsQueryable());

            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Update(It.IsAny<Server>()))
                        .Callback((Server s) => { ser.Add(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Server>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);
            // do not rely on the return value as it is just a placeholder

            //how do I do the ServerPartial?
            //_mockContext.Setup(ctx => ctx.ServersPartial).Returns(_mockServerDbSet.Object);
            //_mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);

        }

        [Test]
        public void ServerEntryDbCheck_TryingToAddAlreadyExistingServer_ShouldNotAddDuplicate()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _serverRepository = new ServerRepository(_mockContext.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            var serverGetAll = _serverRepository.GetAll();
            int initialCount = serverGetAll.Count();
            List<ServerOwnerViewModel> ServersVM = new List<ServerOwnerViewModel>
            {
                new ServerOwnerViewModel{Id = "789317480325316640", ServerPk = 1, Name = "input/output server", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", Approximate_Member_Count=5},
                new ServerOwnerViewModel{Id = "928010025958510632", ServerPk = 2, Name = "Advanced Innovations", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true", Approximate_Member_Count=5},
                new ServerOwnerViewModel{Id = "151516415641361", ServerPk = 3, Name = "fake server", Owner = "false", Icon = "", HasBot = "false", Approximate_Member_Count=23532}
            };

            // Act
            discord.ServerEntryDbCheck(ServersVM[0], "fakeHasbot", "fakeServerOwner");
            var serverGetAll2 = _serverRepository.GetAll();
            int count = serverGetAll.Count();


            // Assert
            Assert.AreEqual(count, initialCount);
        }

        [Test]
        public async Task GetGuilds_404Response_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);


            // Act
            Task<List<Server>?> Act() => discord.GetCurrentUserGuilds("fakeBearerToken");

            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task GetUserInfo_404Response_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            // Act
            Task<DiscordUser?> Act() => discord.GetCurrentUserInfo("fakeBearerToken");

            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task GetUserInfo_NotAuthorizedResponse_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.Unauthorized);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);


            // Act
            Task<DiscordUser?> Act() => discord.GetCurrentUserInfo("fakeBearerToken");

            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task GetCurrentGuild_404Response_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            // Act
            Task<ServerOwnerViewModel?> Act() => discord.GetFullGuild("fakeBotToken", "fakeServerId");

            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task GetCurrentGuild_NotAuthorizedResponse_ShouldThrowException()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.Unauthorized);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);


            // Act
            Task<ServerOwnerViewModel?> Act() => discord.GetFullGuild("fakeBotToken", "fakeServerId");

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

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

        [Test]
        public async Task GetUserInfo_ValidDataForOneUserFromDiscord_ShouldParseOK()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"{
            ""id"": ""1035111022"",
            ""name"": ""test"",
            ""avatar"": ""8342729096ea3675442027381ff50dfe""
            //""owner"": ""true""
            //""permissions"": ""36953089"",
            //""features"": [""COMMUNITY"", ""NEWS""]
            }";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            // Act
            DiscordUser? userInfo = await discord.GetCurrentUserInfo("fakeBearerToken");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userInfo.Id == "1035111022");
                Assert.That(userInfo.Username == "test");
                Assert.That(userInfo.Avatar == "8342729096ea3675442027381ff50dfe");
            }
            );

        }

        [Test]
        public async Task GetCurrentGuild_ValidDataForTwoServersFromDiscord_ShouldParseOK()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"{
            ""id"": ""1035111022"",
            ""name"": ""testServer"",
            ""icon"": ""8342729096ea3675442027381ff50dfe"",
            ""approximate_member_count"": ""2""
            //""permissions"": ""36953089"",
            //""features"": [""COMMUNITY"", ""NEWS""]
            }";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            // Act
            ServerOwnerViewModel? serverInfo = await discord.GetFullGuild("fakeBotToken", "fakeServerId");
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(serverInfo.Id == "1035111022");
                Assert.That(serverInfo.Name == "testServer");
                Assert.That(serverInfo.Icon == "8342729096ea3675442027381ff50dfe");
                Assert.That(serverInfo.ApproximateMemberCount == 2);
            }
            );

        }

        [Test]
        public void GetGuildsInfo_WhereBotIsInServer__ShouldReturnTrue()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK);


            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository);
            var a = discord.CheckForBot("FakeBotToken", "FakeServerId").Result;


            Assert.AreEqual(discord.CheckForBot("FakeBotToken", "FakeServerId").Result, "true");
        }

        [Test] public void GetGuildsInfo_WhereBotIsNotInServer__ShouldReturnFalse()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            Assert.AreEqual(discord.CheckForBot("FakeBotToken", "FakeServerId").Result, "false");
        }

        [Test]
        public async Task AddMemberToGuild_SuccessfullyRespondsWithAlreadyAMemeber()
        {
            // Arrange  (wrap this up in a method to reuse it)
            var handler = new Mock<HttpMessageHandler>();
            AddMemberToPickedServerVM addedMemberProcessInfoVM = new();

            string jsonFromDiscordAPI = @"{
        ""roles"": [], 
        ""nick"": null, 
        ""avatar"": null, 
        ""premium_since"": null, 
        ""joined_at"": ""2022-01-04T19:41:39.926000+00:00"", 
        ""is_pending"": false, 
        ""pending"": false, 
        ""communication_disabled_until"": null, 
        ""user"": {""id"": ""697317543555235840"", 
        ""username"": ""Abraham"", 
        ""avatar"": ""0753a332ab63d2f91971ad57e25123d3"", 
        ""discriminator"": ""7167"", 
        ""public_flags"": 0}, 
        ""mute"": false, 
        ""deaf"": false}";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository);

            // Act
            string? responseInfo = await discord.AddMemberToGuild("fakeBotToken", "fakeServerId");
            var returnAnswer = addedMemberProcessInfoVM.infoOfProcessOfBeingAdded(responseInfo);
            var realAnswer = "You've already joined. From discord:  \r\n        \"joined_at\": \"2022-01-04T19:41:39.926000+00:00\"";

            // Assert
            Assert.AreEqual(returnAnswer, realAnswer); 
        }

    }
}
