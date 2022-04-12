using DiscordStats.DAL.Abstract;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using DiscordStats.ViewModel;
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


using Microsoft.Extensions.Configuration;
using DiscordStats.ViewModels;
using DiscordStats.Controllers;
using Newtonsoft.Json;

namespace DiscordStats_Tests
{
    public class Sprint4_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;
        private Mock<DiscordDataDbContext> _mockContext2;
        private Mock<DiscordDataDbContext> _mockContext3;
        private Mock<DiscordDataDbContext> _mockContextEmpty;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private Mock<DbSet<Channel>> _mockChannelDbSet;
        private Mock<DbSet<Presence>> _mockPresenceDbSet;

        private IServerRepository _serverRepository;
        private IChannelRepository _channelRepository;
        private IPresenceRepository _presenceRepository;
        private IDiscordServicesForChannels _discordServicesForChannels;

        DateTime now = DateTime.Now;

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
                new Channel{Id = "789317480803074075", Type = "Guild_Text", Name = "Text Channels", Count = 400, GuildId= "789317480325316646"},
                new Channel{Id = "12351251452136", Type = "Guild_Voice", Name = "Voice Channels", Count = 220, GuildId= "789317480325316646"}
            };

            var pre = new List<Presence>
            {
                new Presence{Id = "789317480325316640",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="789317480325316640", UserId="41351461513541"},
                new Presence{Id = "789317480325316641",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="789317480325316640", UserId="41351461513542"},
                new Presence{Id = "789317480325316642",  ApplicationId= null, Name = "COD", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="789317480325316640", UserId="41351461513543"},
                new Presence{Id = "789317480325316643",  ApplicationId= null, Name = "Microsoft Edge", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125128", UserId="41351461513544"},
                new Presence{Id = "789317480325316644",  ApplicationId= null, Name = "Calculator", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125129", UserId="41351461513545"},
            };
            var preEmpty = new List<Presence>
            {
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

            _mockPresenceDbSet = GetMockDbSet<Presence>(pre.AsQueryable<Presence>());
            _mockContext3 = new Mock<DiscordDataDbContext>();
            _mockContext3.Setup(ctx => ctx.Presences).Returns(_mockPresenceDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Set<Presence>()).Returns(_mockPresenceDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Update(It.IsAny<Presence>()))
                        .Callback((Presence p) => { pre.Append(p); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Presence>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext3.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockPresenceDbSet = GetMockDbSet<Presence>(preEmpty.AsQueryable<Presence>());
            _mockContextEmpty = new Mock<DiscordDataDbContext>();
            _mockContextEmpty.Setup(ctx => ctx.Presences).Returns(_mockPresenceDbSet.Object);
            _mockContextEmpty.Setup(ctx => ctx.Set<Presence>()).Returns(_mockPresenceDbSet.Object);
            _mockContextEmpty.Setup(ctx => ctx.Update(It.IsAny<Presence>()))
                        .Callback((Presence p) => { preEmpty.Append(p); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Presence>)null);
            // do not rely on the return value from Update since it's just null
            _mockContextEmpty.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

        }

        [Test]
        public async Task AllServers_ShouldReturnTrueForModelNotNull()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _serverRepository = new ServerRepository(_mockContext.Object);
            _presenceRepository = new PresenceRepository(_mockContext3.Object);


            HomeController controller = new HomeController(null, _serverRepository, _presenceRepository, null, null);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.AllServers();
            var modelTrueIfNotNull = !result.Model.Equals(null);


            // Assert
            Assert.AreEqual(true, modelTrueIfNotNull);
        }

        [Test]
        public void AllServersVM_WithPresenceInServer_ShouldReturnServerOwnerViewModelCountOfFive()
        {
            // Arrange
            _serverRepository = new ServerRepository(_mockContext.Object);
            _presenceRepository = new PresenceRepository(_mockContext3.Object);
            AllServersVM allServersVM = new(_serverRepository, _presenceRepository);

            // Act
            IList<ServerOwnerViewModel> listOfServerOwnerViewModel = allServersVM.AllServerNameAndMemCountContainer();
            int VMCount = listOfServerOwnerViewModel.Count();
            // Assert
            Assert.AreEqual(5, VMCount);
        }

        [Test]
        public void AllServersVM_WithNoPresenceInServer_ShouldReturnServerOwnerViewModelCountOfFive()
        {
            // Arrange
            _serverRepository = new ServerRepository(_mockContext.Object);
            _presenceRepository = new PresenceRepository(_mockContextEmpty.Object);
            AllServersVM allServersVM = new(_serverRepository, _presenceRepository);

            // Act
            IList<ServerOwnerViewModel> listOfServerOwnerViewModel = allServersVM.AllServerNameAndMemCountContainer();
            int VMCount = listOfServerOwnerViewModel.Count();
            // Assert
            Assert.AreEqual(5, VMCount);
        }

        [Test]
        public void PresenceRepository_ShouldReturnFoundPresenceName()
        {
            // Arrange
            _presenceRepository = new PresenceRepository(_mockContext3.Object);
            var pre = new List<Presence>
            {
                new Presence{Id = "789317480325316640",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="789317480325316640", UserId="41351461513541"},
            };

            // Act
            Presence presence = _presenceRepository.FindPresence(pre[0].Name);

            //Assert
            Assert.AreEqual("Microsoft Visual Studion", presence.Name);

        }


[Test]
        public async Task GetWebHooks_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            Task<List<WebhookUsageVM>?> Act() => discord.GetChannelWebHooks("fakeBotToken", "FakeChannelId");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }

        [Test]
        public async Task PostWebHookToCreate_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            Task<string?> Act() => discord.CreateWebhook("fakeBotToken", "FakeChannelId", "FakeWebHookName");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }

        [Test]
        public async Task PostWebHookToSendMessage_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            Task<string?> Act() => discord.SendMessageThroughWebhook("fakeBotToken", "FakeWebHooklId", "fakeWebHookToken", "FakeMessage");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }

        [Test]
        public async Task WebHookToDelete_404Response_ShouldThrowException()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            Task<string?> Act() => discord.DeleteWebhook("fakeBotToken", "webhookId");
            // Assert
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());

        }

        [Test]
        public async Task GetWebHooks_ShouldParseOk()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var whatetever = _channelRepository.GetAll().ToList();


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

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);
            List<WebhookUsageVM>? webhookUsageVm = new();

            // Act
            webhookUsageVm = await discord.GetChannelWebHooks("fakeBotToken","fakeChannelID");
           

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(webhookUsageVm!.Count, Is.EqualTo(2));
                Assert.That(webhookUsageVm[0].channel_id == "789317480803074075");
                Assert.That(webhookUsageVm[0].name == "Captain Hook");
                Assert.That(webhookUsageVm[0].guild_id == "789317480325316640");
            });

        }

        [Test]
        public async Task PostWebHookToCreate_ReturnsSuccessMessage()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"[{
                ""type"": 1, 
                    ""id"": ""961320916732756009"", 
                    ""name"": ""a"", 
                    ""avatar"": null, 
                    ""channel_id"": ""952963443336564766"", 
                    ""guild_id"": ""789317480325316640"", 
                    ""application_id"": ""938808510932746350"", 
                    ""token"": ""oVK2tmq-DjHEl6w15Zf-N4ndWqDJdRchFtKcfV8aFx4vBy125rkLu_fFxvt5zYIqK3w-"",
                    ""user"": 
                    {
                    ""id"": ""938808510932746350"", 
                       ""username"": ""Stat Tracker"", 
                        ""avatar"": null, 
                        ""discriminator"": ""7079"", 
                        ""public_flags"": 0, 
                        ""bot"": true
                    }
            }]";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            string? webhook = await discord.CreateWebhook("fakeBotToken", "fakeChannelId", "fakeWebHookName");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(webhook.Contains("961320916732756009"));
                Assert.That(webhook.Contains("a")); 
            });

        }

        [Test]
        public async Task PostWebHookToSendMessage_ReturnsSuccessMessage()
        {
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"[{
                ""type"": 1, 
                    ""id"": ""961320916732756009"", 
                    ""name"": ""a"", 
                    ""avatar"": null, 
                    ""channel_id"": ""952963443336564766"", 
                    ""guild_id"": ""789317480325316640"", 
                    ""application_id"": ""938808510932746350"", 
                    ""token"": ""oVK2tmq-DjHEl6w15Zf-N4ndWqDJdRchFtKcfV8aFx4vBy125rkLu_fFxvt5zYIqK3w-"",
                    ""user"": 
                    {
                    ""id"": ""938808510932746350"", 
                       ""username"": ""Stat Tracker"", 
                        ""avatar"": null, 
                        ""discriminator"": ""7079"", 
                        ""public_flags"": 0, 
                        ""bot"": true
                    }
            }]";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            string? webhook = await discord.SendMessageThroughWebhook("fakeBotToken", "fakewebhookId", "fakewebhookToken", "FakeMessage");


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(webhook.Contains("961320916732756009"));
                Assert.That(webhook.Contains("a"));
            });

        }

        [Test]
        public async Task WebHookToDelete_ReturnsEmptyMessage()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            string? returnValue = await discord.DeleteWebhook("fakeBotToken", "fakewebhookId");

            // Assert
            Assert.AreEqual("", returnValue);

        }

        [Test]
        public async Task ChannelWebHooks_ShouldReturnListOfWebhooks()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            string jsonFromDiscordAPI = @"[{
            ""type"": 1,
            ""id"": ""933605549457682442"",
            ""name"": ""Captain hook"",
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

            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480803074075").FirstOrDefault();
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.ChannelWebhooks(selectedChannel);
            var vmCompare = new List<WebhookUsageVM>()
            {
                new WebhookUsageVM
                {
                    channelId = null,
                    channel_id = "789317480803074075",
                    guild_id = "789317480325316640",
                    name = "Captain hook",
                    Id = "933605549457682442",
                    Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk",
                    message = null,
                    memberValue = false,
                    activityValue = false,
                },
                new WebhookUsageVM
                {
                    channelId = null,
                    channel_id = "789317480803074075",
                    guild_id = "789317480325316640",
                    name = "githubConnectTest",
                    Id = "933799816499187743",
                    Token = "hYAg9k6mFxVsUTR66EVqO_837ngFTT_98pDDHuYqqRdzlMdVcTyUS6s3f8HJfz1aNjU7",
                    message = null,
                    memberValue = false,
                    activityValue = false,
                },

            };

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(result.Model);
            var actualJson = System.Text.Json.JsonSerializer.Serialize(vmCompare);

            // Assert
            Assert.AreEqual(actualJson, expectedJson);
        }

        [Test]
        public async Task WebHookForm_ShouldReturnDataCountOfOneToPassToTheViewPage()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480803074075").FirstOrDefault();
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            ViewResult result = (ViewResult)await controller.WebhookForm(selectedChannel.Id);
            var dataToReturnToViewHasCount = result.ViewData.Count();         

            // Assert
            Assert.AreEqual(1, dataToReturnToViewHasCount);
        }

        [Test]
        public async Task WebhookFormToCreateAWebhook_ShouldRedirectToWebhookMessageActionResultWithObjectNameOnly()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            string jsonFromDiscordAPI = @"{
            ""id"": ""933605549457682442"",
            ""type"": ""1"",
            ""name"": ""Captain hook"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk"",
            }";
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480803074075").FirstOrDefault();
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            WebhookUsageVM vm = new WebhookUsageVM();

            vm.channelId = null;
            vm.channel_id = "789317480803074075";
            vm.guild_id = "789317480325316640";
            vm.name = "Captain hook";
            vm.Id = "933605549457682442";
            vm.Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk";
            vm.message = null;
            vm.memberValue = false;
            vm.activityValue = false;

            RedirectToActionResult result = (RedirectToActionResult)await controller.WebhookForm(vm);
            string actionName = result.ActionName;
            var routeValueObjectName = result.RouteValues.Values.ToArray()[3];
           

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("WebhookMessage", actionName);
                Assert.AreEqual("Captain hook", routeValueObjectName);
            });
        }

        [Test]
        public async Task WebhookMessageToShowWhichWebhookInUse_ShouldReturnWebHookUsageVM()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            ChannelController controller = new ChannelController(null, null, null, _discordServicesForChannels, _serverRepository);
            controller.ControllerContext = new ControllerContext();
            WebhookUsageVM vm = new WebhookUsageVM();
            vm.Id = "933605549457682442";
            vm.name = "Captain Hook";
            vm.Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk";
            vm.channel_id = "789317480803074075";
            vm.guild_id = "789317480325316640";


            // Act
            ViewResult result = (ViewResult)await controller.WebhookMessage(vm);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(result.Model);
            var actualJson = System.Text.Json.JsonSerializer.Serialize(vm);

            // Arrange
            Assert.AreEqual(expectedJson, actualJson);
        }

        [Test]
        public async Task WebhookMessageAfterUserChoosesDataToSend_ShouldReturnWebhookMessageViewPageWithModelNull()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            string jsonFromDiscordAPI = @"{
            ""id"": ""933605549457682442"",
            ""type"": ""1"",
            ""name"": ""Captain hook"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk"",
            }";
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            _serverRepository = new ServerRepository(_mockContext.Object);
            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            WebhookUsageVM vm = new WebhookUsageVM();

            vm.channelId = null;
            vm.channel_id = "789317480803074075";
            vm.guild_id = "789317480325316640";
            vm.name = "Captain hook";
            vm.Id = "933605549457682442";
            vm.Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk";
            vm.message = null;
            vm.memberValue = false;
            vm.activityValue = false;

            ViewResult result = (ViewResult)await controller.WebhookMessage(vm, "");

            // Assert
            Assert.AreEqual(null, result.Model);
        }

        [Test]
        public async Task DeleteWebhook_ShouldRedirectToChannelWebhooksActionResultWithChannelObjectId()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            string jsonFromDiscordAPI = @"{
            ""id"": ""933605549457682442"",
            ""type"": ""1"",
            ""name"": ""Captain hook"",
            ""avatar"": null,
            ""channel_id"": ""789317480803074075"",
            ""guild_id"": ""789317480325316640"",
            ""application_id"": null,
            ""token"": ""V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk"",
            }";
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository);
            controller.ControllerContext = new ControllerContext();

            // Act
            WebhookUsageVM vm = new WebhookUsageVM();

            vm.channelId = null;
            vm.channel_id = "789317480803074075";
            vm.guild_id = "789317480325316640";
            vm.name = "Captain hook";
            vm.Id = "933605549457682442";
            vm.Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk";
            vm.message = null;
            vm.memberValue = false;
            vm.activityValue = false;

            RedirectToActionResult result = (RedirectToActionResult)await controller.DeleteWebhook(vm);
            string actionName = result.ActionName;
            var routeValueObjectName = result.RouteValues.Values.ToArray()[1];


            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("ChannelWebhooks", actionName);
                Assert.AreEqual("789317480803074075", routeValueObjectName);
            });
        }

        [Test]
        public async Task WebhookDataVm_ShouldReturnMemberValue()
        {
            // Arange
            _serverRepository = new ServerRepository(_mockContext.Object);
            _channelRepository = new ChannelRepository(_mockContext2.Object);

            WebhookDataVm webhookDataVm = new(_serverRepository, _channelRepository);

            // Act 
            WebhookUsageVM vm = new WebhookUsageVM();

            vm.channelId = null;
            vm.channel_id = "789317480803074075";
            vm.guild_id = "789317480325316640";
            vm.name = "Captain hook";
            vm.Id = "933605549457682442";
            vm.Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk";
            vm.message = null;
            vm.memberValue = true;
            vm.activityValue = false;

            var memberValue = webhookDataVm.DataBeingSentBackForWebhook(vm);

            // Assert
            Assert.AreEqual("There are 500 members in this server", memberValue);
        }

        [Test]
        public async Task WebhookDataVm_ShouldReturnActivityValue()
        {
            // Arange
            _serverRepository = new ServerRepository(_mockContext.Object);
            _channelRepository = new ChannelRepository(_mockContext2.Object);

            WebhookDataVm webhookDataVm = new(_serverRepository, _channelRepository);

            // Act 
            WebhookUsageVM vm = new WebhookUsageVM();

            vm.channelId = null;
            vm.channel_id = "789317480803074075";
            vm.guild_id = "789317480325316640";
            vm.name = "Captain hook";
            vm.Id = "933605549457682442";
            vm.Token = "V42jNvQvkQQjum7BUuuD5p8y0EPHROacQJwtx8gOTemg4kcROO5Gq5J8cjj-irG_dTGk";
            vm.message = null;
            vm.memberValue = false;
            vm.activityValue = true;

            var activityValue = webhookDataVm.DataBeingSentBackForWebhook(vm);

            // Assert
            Assert.AreEqual("The message count in Text Channels is 400", activityValue);

        }
    }
}
