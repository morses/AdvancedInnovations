using DiscordStats.DAL.Abstract;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Contrib.HttpClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStats_Tests
{
    public class AD_166_CS_461Final_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;
        private Mock<DiscordDataDbContext> _mockContext2;
        private Mock<DiscordDataDbContext> _mockContext3;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private Mock<DbSet<Channel>> _mockChannelDbSet;
        private Mock<DbSet<WebHook>> _mockWebHookDbSet;
        //private List<Server> _servers = FakeData.Servers;

        private IServerRepository _serverRepository;
        private IChannelRepository _channelRepository;
        private IWebHookRepository _webHookRepository;

        [SetUp]
        public void Setup()
        {
            var ser = new List<Server>
             {
                new Server{Id = "789317480325316646", ServerPk = 7, Name = "Seveth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=25, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316647", ServerPk = 8, Name = "Eith Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=20, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316642", ServerPk = 3, Name = "Third Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=300, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316643", ServerPk = 4, Name = "Fourth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=200, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316641", ServerPk = 2, Name = "Second Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=400, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="true",Message="bye"},
                new Server{Id = "789317480325316645", ServerPk = 6, Name = "Sixth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=50, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316648", ServerPk = 9, Name = "Ninth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=5, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316644", ServerPk = 5, Name = "Fifth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=100, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
            };

            var cha = new List<Channel>
             {
                new Channel{Id = "12351251452135", Type = "Guild_Text", Name = "Text Channels", Count = 400, GuildId= "789317480325316640"},
                new Channel{Id = "12351251452136", Type = "Guild_Voice", Name = "Voice Channels", Count = 220, GuildId= "789317480325316641"}
            };

            var webH = new List<WebHook>
             {
                new WebHook{Type = 1, Id = "933605549457682442", Type = "Guild_Text", Name = "Captain Hook", channel_id = "789317480803074075", GuildId= "789317480325316640", Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk"},
                new WebHook{Type = 2, Id = "933605549457682443", Type = "Guild_Text", Name = "tester hook", channel_id = "789317480803074076", GuildId= "789317480325316640", Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGl"},
            };

            _mockServerDbSet = GetMockDbSet<Server>(ser.AsQueryable<Server>());
            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Update(It.IsAny<Server>()))
                        .Callback((Server s) => { ser.Append(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Server>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockChannelDbSet = GetMockDbSet<Channel>(cha.AsQueryable<Channel>());
            _mockContext2 = new Mock<DiscordDataDbContext>();
            _mockContext2.Setup(ctx => ctx.Channels).Returns(_mockChannelDbSet.Object);
            _mockContext2.Setup(ctx => ctx.Set<Channel>()).Returns(_mockChannelDbSet.Object);
            _mockContext2.Setup(ctx => ctx.Update(It.IsAny<Channel>()))
                        .Callback((Channel c) => { cha.Append(c); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Channel>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext2.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockWebHookDbSet = GetMockDbSet<WebHook>(cha.AsQueryable<WebHook>());
            _mockContext3 = new Mock<DiscordDataDbContext>();
            _mockContext3.Setup(ctx => ctx.WebHooks).Returns(_mockWebHookDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Set<WebHook>()).Returns(_mockWebHookDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Update(It.IsAny<WebHook>()))
                        .Callback((Channel c) => { webH.Append(c); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<WebHook>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext3.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);


        }

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            //mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => entities.GetEnumerator());
            return mockSet;
        }

        [Test]
        public async Task GetWebHooks_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, _channelRepository);

            // Act
            Task<List<Channel>?> Act() => discord.GetWebHooks("fakeBotToken", "FakeChannelId");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }

        [Test]
        public async Task PosttWebHookToCreate_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, _channelRepository);

            // Act
            Task<List<Channel>?> Act() => discord.PosttWebHookToCreate("fakeBotToken", "FakeChannelId", "FakeWebHookName");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }
        [Test]
        public async Task PosttWebHookToSendMessage_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, _channelRepository);

            // Act
            Task<List<Channel>?> Act() => discord.PosttWebHookToSendMessage("fakeBotToken", "fakeChannelToken", "FakeWebHooklId", "FakeMessage");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }
        [Test]
        public async Task GetWebHooks_ShouldParseOk()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"[{
            ""type"": 1,
            ""id"": ""933605549457682442"",
            ""name"": ""Captain Hook"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk"",
            ""user"": {
                    ""id"": ""697317543555235840"",
                ""username"": ""Abraham"",
                ""avatar"": ""0753a332ab63d2f91971ad57e25123d3"",
                ""discriminator"": ""7167"",
                ""public_flags"": 0
                },
            {
            ""type"": 1,
            ""id"": ""933799816499187743"",
            ""name"": ""githubConnectTest"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""hYAg9k6mFxVsUTR66EVqO_837ngFTT_98pDDHuYqqRdzlMdVcTyUS6s3f8HJfz1aNjU7"",
            ""user"": {
                ""id"": ""697317543555235840"",
                ""username"": ""Abraham"",
                ""avatar"": ""0753a332ab63d2f91971ad57e25123d3"",
                ""discriminator"": ""7167"",
                ""public_flags"": 0
                }
            }]";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, _channelRepository);

            // Act
            List<Channel>? channels = discord.GetWebHooks("fakeBotToken", "FakeChannelId");
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(channels!.Count, Is.EqualTo(2));
                Assert.That(channels[0].channel_id == "789317480803074075");
                Assert.That(channels[0].Name == "Captain Hook");
                Assert.That(channels[0].GuildId == "789317480325316640");
            }

        }

        [Test]
        public async Task PosttWebHookToCreate_ReturnsSuccessMessage()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, _channelRepository);

            // Act
            string success = discord.PosttWebHookToCreate("fakeBotToken", "FakeChannelId", "FakeWebHookName");
            string compareString = "WebHook was created.";
            
            // Assert
            Assert.AreEqual(success, compareString);

        }
        [Test]
        public async Task PosttWebHookToSendMessage_ReturnsSuccessMessage()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, _channelRepository);

            // Act
            string success = discord.PosttWebHookToSendMessage("fakeBotToken", "fakeChannelToken", "FakeWebHooklId", "FakeMessage");
            string compareString = "Message was sent.";

            // Assert
            Assert.AreEqual(success, compareString);

        }

        [Test]
        public async Task PostChannels_ShouldReturnItWorked()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext.Object);

            ChannelController controller = new ChannelController(null, null, null, _serverRepository, _channelRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            JsonResult result = (JsonResult)await controller.ServerChannels(_channelRepository.GetAll());
            var resultCompare = "It Worked";

            // Arrange
            Assert.AreEqual(result, resultCompare);
        }

        [Test]
        public async Task ServerTextChannels_ShouldReturnListOfTextChannels()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext.Object);
            _serverRepository = new ServerRepository(_mockContext.Object);
            var selectedServer = _serverRepository.GetAll().Where(m => m.Id == "789317480325316640").FirstOrDefault();

            ChannelController controller = new ChannelController(null, null, null, _serverRepository, _channelRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.ServerTextChannels(selectedServer.Id);
            ChannelListPageVM vm = (ChannelListPageVM)result.Model;

            string resultCompare = @"[{
                    ""id"": ""789317480803074073"",
                    ""type"": 4,
                    ""name"": ""Text Channels"",
                    ""position"": 0,
                    ""parent_id"": null,
                    ""guild_id"": ""789317480325316640"",
                    ""permission_overwrites"": []
                },

                {
                    ""id"": ""952963443336564766"",
                    ""last_message_id"": ""953521469462290503"",
                    ""type"": 0,
                    ""name"": ""testchannel"",
                    ""position"": 1,
                    ""parent_id"": ""789317480803074073"",
                    ""topic"": null,
                    ""guild_id"": ""789317480325316640"",
                    ""permission_overwrites"": [],
                    ""rate_limit_per_user"": 0,
                    ""nsfw"": false
                }
            ]";

            // Assert
            Assert.AreEqual(vm.ListOfTextChannels, resultCompare);
        }

        public async Task WebHooks_ShouldReturnListOfWebhooks()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext.Object);
            _serverRepository = new ServerRepository(_mockContext.Object);
            var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480325316640").FirstOrDefault();

            ChannelController controller = new ChannelController(null, null, null, _serverRepository, _channelRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.ChannelWebHooks(selectedChannel.Id);
            WebHooklListPageVM vm = (WebHooklListPageVM)result.Model;

            string resultCompare = @"[{
            ""type"": 1,
            ""id"": ""933605549457682442"",
            ""name"": ""Captain Hook"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk"",
            ""user"": {
                    ""id"": ""697317543555235840"",
                ""username"": ""Abraham"",
                ""avatar"": ""0753a332ab63d2f91971ad57e25123d3"",
                ""discriminator"": ""7167"",
                ""public_flags"": 0
                },
            {
            ""type"": 1,
            ""id"": ""933799816499187743"",
            ""name"": ""githubConnectTest"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""hYAg9k6mFxVsUTR66EVqO_837ngFTT_98pDDHuYqqRdzlMdVcTyUS6s3f8HJfz1aNjU7"",
            ""user"": {
                ""id"": ""697317543555235840"",
                ""username"": ""Abraham"",
                ""avatar"": ""0753a332ab63d2f91971ad57e25123d3"",
                ""discriminator"": ""7167"",
                ""public_flags"": 0
                }
            }]";

            // Assert
            Assert.AreEqual(vm.ListOfWebHooks, resultCompare);
        }

        [Test]
        public async Task SelectedWebHookController_ToSendMessage_ShouldReturnSuccess()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            _webHookRepository = new IWebHookRepository(_mockContext3.Object);
            var selectedWebhook = _webHookRepository.GetAll().Where(m => m.Id == "933605549457682442").FirstOrDefault();

            ChannelController controller = new ChannelController(null, null, null, _serverRepository, _channelRepository, _webHookRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.SelectedWebHook(selectedWebhook.Id, selectedWebhook.Token, "fakeMessageToSend");

            var resultCompare = "Message was sent.";

            // Arrange
            Assert.AreEqual(result, resultCompare);
        }

        [Test]
        public async Task CreatingWebHookController_ShouldReturnWebHookObject()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext.Object);
            var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480325316640").FirstOrDefault();

            ChannelController controller = new ChannelController(null, null, null, _serverRepository, _channelRepository, _webHookRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.CreateWebHook(selectedChannel.Id);

            var resultCompare = "WebHook was created.";

            // Arrange
            Assert.AreEqual(result, resultCompare);
        }
    }
}
