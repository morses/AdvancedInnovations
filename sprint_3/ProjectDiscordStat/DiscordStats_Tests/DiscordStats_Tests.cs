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
using DiscordStats.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DiscordStats_Tests
{
    public class DiscordStats_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;
        private Mock<DiscordDataDbContext> _mockContext1;
        private Mock<DiscordDataDbContext> _mockContext2;
        private Mock<DiscordDataDbContext> _mockContext3;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private Mock<DbSet<Presence>> _mockPresenceDbSet;
        private Mock<DbSet<Channel>> _mockChannelDbSet;
        private Mock<DbSet<VoiceChannel>> _mockVoiceChannelDbSet;
        private List<Server> _servers = FakeData.Servers;

        private IServerRepository _serverRepository;
        private IPresenceRepository _presenceRepository;
        private IChannelRepository _channelRepository;
        private IVoiceChannelRepository _voiceChannelRepository;

        DateTime now = DateTime.Now;

      
        //private Mock<DbSet<ServerPartial>> _mockPartialServerDbSet;
        //private List<ServerPartial> _serverPartial = FakeData.ServersfromPartial;

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
            var _VoiceChannels = new List<VoiceChannel>
            {
                new VoiceChannel{Id = "156209528969494528", Name ="General", Count=1,GuildId="928010025958510632",Time=DateTime.Parse("2022-04-06 20:17:38.263") }
            };
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

            var pre = new List<Presence>()
            {
                new Presence{Id = "789317480325316640",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125125", UserId="41351461513541"},
                new Presence{Id = "789317480325316641",  ApplicationId= null, Name = "Azure Data Studio", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125126", UserId="41351461513542"},
                new Presence{Id = "789317480325316642",  ApplicationId= null, Name = "COD", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125127", UserId="41351461513543"},
                new Presence{Id = "789317480325316643",  ApplicationId= null, Name = "Microsoft Edge", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125128", UserId="41351461513544"},
                new Presence{Id = "789317480325316644",  ApplicationId= null, Name = "Calculator", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125129", UserId="41351461513545"},
            };

            var cha = new List<Channel>
             {
                new Channel{Id = "12351251452135", Type = "Guild_Text", Name = "Text Channels", Count = 400, GuildId= "789317480325316640"},
                new Channel{Id = "12351251452136", Type = "Guild_Voice", Name = "Voice Channels", Count = 220, GuildId= "789317480325316641"}
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

            _mockPresenceDbSet = GetMockDbSet<Presence>(pre.AsQueryable<Presence>());
            _mockContext1 = new Mock<DiscordDataDbContext>();
            _mockContext1.Setup(ctx => ctx.Presences).Returns(_mockPresenceDbSet.Object);
            _mockContext1.Setup(ctx => ctx.Set<Presence>()).Returns(_mockPresenceDbSet.Object);
            _mockContext1.Setup(ctx => ctx.Update(It.IsAny<Presence>()))
                        .Callback((Presence p) => { pre.Append(p); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Presence>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext1.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);
            _mockContext1.Setup(x => x.Add(It.IsAny<Presence>())).Callback<Presence>((s) => pre.Add(s));
            _mockContext1.Setup(x => x.Update(It.IsAny<Presence>())).Callback<Presence>((s) =>
            {
                var found = pre.FirstOrDefault(a => a.Id == s.Id);
                if (found == null)
                {
                    pre.Add(s);
                }
                else
                {
                    var item = pre.Where(a => a.Id == s.Id).First();
                    var index = pre.IndexOf(item);
                    pre[index] = s;
                }
            });
        

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









                                            //Voice Channels
            _mockVoiceChannelDbSet = GetMockDbSet<VoiceChannel>(_VoiceChannels.AsQueryable<VoiceChannel>());
            _mockContext3 = new Mock<DiscordDataDbContext>();
            _mockContext3.Setup(ctx => ctx.VoiceChannels).Returns(_mockVoiceChannelDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Set<VoiceChannel>()).Returns(_mockVoiceChannelDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Update(It.IsAny<VoiceChannel>()))
                        .Callback((VoiceChannel p) => { _VoiceChannels.Append(p); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<VoiceChannel>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext3.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);
            _mockContext3.Setup(x => x.Add(It.IsAny<VoiceChannel>())).Callback<VoiceChannel>((s) => _VoiceChannels.Add(s));
            _mockContext3.Setup(x => x.Update(It.IsAny<VoiceChannel>())).Callback<VoiceChannel>((s) =>
            {
                var found = pre.FirstOrDefault(a => a.Id == s.Id);
                if (found == null)
                {
                    _VoiceChannels.Add(s);
                }
                else
                {
                    var item = _VoiceChannels.Where(a => a.Id == s.Id).First();
                    var index = _VoiceChannels.IndexOf(item);
                    _VoiceChannels[index] = s;
                }
            });
        }

        [Test]
        public void GetServerDataFromDb_ShouldReturnTheTopFivePublicServersByMemberCount()
        {
            // Arrange
            _serverRepository = new ServerRepository(_mockContext.Object);
            var comparedServer = new List<Server>()
            {
                new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316641", ServerPk = 2, Name = "Second Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=400, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316642", ServerPk = 3, Name = "Third Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=300, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316643", ServerPk = 4, Name = "Fourth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=200, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316644", ServerPk = 5, Name = "Fifth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=100, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316645", ServerPk = 6, Name = "Sixth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=50, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316646", ServerPk = 7, Name = "Seveth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=25, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
            };

            // Act
            IList<Server> servers = _serverRepository.GetAll().Where(a => a.Privacy == "public").OrderByDescending(m => m.ApproximateMemberCount).Take(5).ToList();
            IList<Server> comparedServerGraphReady = _serverRepository.GetAll().Where(a => a.Privacy == "public").OrderByDescending(m => m.ApproximateMemberCount).Take(5).ToList();


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(comparedServerGraphReady.Count == servers.Count);
                Assert.That(comparedServerGraphReady[0].ServerPk == servers[0].ServerPk);
                Assert.That(comparedServerGraphReady[3].ApproximateMemberCount == servers[3].ApproximateMemberCount);
            });
        }

        [Test]
        public void PresenceEntryDbCheck_TryingToAddAlreadyExistingServer_ShouldNotAddDuplicate()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _presenceRepository = new PresenceRepository(_mockContext1.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null);

            var presenceGetAll = _presenceRepository.GetAll();
            int initialCount = presenceGetAll.Count();
            var presence = new Presence[]
            {
                new Presence{Id = "789317480325316640",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125125", UserId="41351461513541"},
                new Presence{Id = "789317480325316641",  ApplicationId= null, Name = "Azure Data Studio", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125126", UserId="41351461513542"},
                new Presence{Id = "789317480325316642",  ApplicationId= null, Name = "COD", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125127", UserId="41351461513543"},
                new Presence{Id = "789317480325316643",  ApplicationId= null, Name = "Microsoft Edge", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125128", UserId="41351461513544"},
                new Presence{Id = "789317480325316644",  ApplicationId= null, Name = "Calculator", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125129", UserId="41351461513545"},
            };

            // Act
            discord.PresenceEntryAndUpdateDbCheck(presence);
            var serverGetAll2 = _presenceRepository.GetAll();
            int count = presenceGetAll.Count();


            // Assert
            Assert.AreEqual(count, initialCount);
        }

        [Test]
        public void AllPresenceNameListAndCount_ShouldReturnPresencesInKeyValuePairOfNameToCount()
        {
            // Arrange
            PresenceChartDataVM presenceChartDataVM = new();
            var presence = new List<Presence>
            {
                new Presence{Id = "789317480325316640",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125125", UserId="41351461513541"},
                new Presence{Id = "789317480325316641",  ApplicationId= null, Name = "Azure Data Studio", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125126", UserId="41351461513542"},
                new Presence{Id = "789317480325316642",  ApplicationId= null, Name = "Microsoft Visual Studion", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125127", UserId="41351461513543"},
                new Presence{Id = "789317480325316643",  ApplicationId= null, Name = "Azure Data Studio", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125128", UserId="41351461513544"},
                new Presence{Id = "789317480325316644",  ApplicationId= null, Name = "Azure Data Studio", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125129", UserId="41351461513545"},
            };

            Dictionary<string, int> orderShouldBe = new Dictionary<string, int>();
            orderShouldBe.Add("Microsoft Visual Studion", 2);
            orderShouldBe.Add("Azure Data Studio", 3);


            // Act
            var presenceReturn = presenceChartDataVM.AllPresenceNameListAndCount(presence);
            // 
            Assert.AreEqual(orderShouldBe, presenceReturn);

        }

        [Test]
        public void UpdateOnServerWithForumInfo_ShouldReturnUpdatedOnForumStatusAndMessage()
        {
            // Arrange
            _serverRepository = new ServerRepository(_mockContext.Object);
            var servers = _serverRepository.GetAll();
            var comparedServer = new List<Server>()
            {
                new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="true",Message="hello"},
            };

            // Act
            foreach (var i in servers)
            {
                if (i.Id == "789317480325316640")
                {
                    _serverRepository.UpdateOnServerWithForumInfo("789317480325316640", i.OnForum = "true", i.Message = "hello");
                }
            }
            var compare = comparedServer.ElementAt(0);
            var result = servers.ElementAt(2);

            // Assert         
            Assert.Multiple(() =>
            {
                Assert.AreEqual(compare.Id, result.Id);
                Assert.AreEqual(compare.OnForum, result.OnForum);
                Assert.AreEqual(compare.Message, result.Message);
            });
        }

        [Test]
        public void ChannelsEntryFromBotToDbCheck_TryingToAddAlreadyExistingChannel_ShouNotAddDuplicate()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _channelRepository = new ChannelRepository(_mockContext2.Object);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);


            var channelGetAll = _channelRepository.GetAll();
            int initialCount = channelGetAll.Count();
            var channel = new Channel[]
            {
                new Channel{Id = "12351251452135", Type = "Guild_Text", Name = "Text Channels", Count = 400, GuildId= "789317480325316640"},
            };

            // Act
            discord.ChannelEntryAndUpdateDbCheck(channel);
            var channelGetAll2 = _channelRepository.GetAll();
            int count = channelGetAll.Count();


            // Assert
            Assert.AreEqual(count, initialCount);
        }


        //[Test]
        //public async Task GetGuildChannelsWithApi_ShouldReturnCountOfChannelsAndInfoOfChannelOne()
        //{
        //    var handler = new Mock<HttpMessageHandler>();

        //    string jsonFromDiscordAPI = @"[{
        //    ""id"": ""1035111022"",
        //    ""type"": 4,
        //    ""name"": ""Text Channels"",
        //    ""position"": 0,
        //    ""parent_id"": null,
        //    ""guild_id"": ""213514616"",
        //    ""permission_overwrites"": [],
        //    ""nsfw"": false },
        //    {
        //    ""id"": ""435342515"",
        //    ""type"": 4,
        //    ""name"": ""Voice Channels"",
        //    ""position"": 0,
        //    ""parent_id"": null,
        //    ""guild_id"": ""213514616"",
        //    ""permission_overwrites"": [],
        //    ""nsfw"": false 
        //    }
        //    ]";

        //    var response = new HttpResponseMessage()
        //    {
        //        Content = new StringContent(jsonFromDiscordAPI)
        //    };
        //    handler.SetupAnyRequest()
        //            .ReturnsAsync(response);

        //    DiscordService discord = new DiscordService(handler.CreateClientFactory(), null, null, null,null);


        //    string jsonFromDiscordAPI = @"[{
        //    ""id"": ""1035111022"",
        //    ""type"": 4,
        //    ""name"": ""Text Channels"",
        //    ""position"": 0,
        //    ""parent_id"": null,
        //    ""guild_id"": ""213514616"",
        //    ""permission_overwrites"": [],
        //    ""nsfw"": false },
        //    {
        //    ""id"": ""435342515"",
        //    ""type"": 4,
        //    ""name"": ""Voice Channels"",
        //    ""position"": 0,
        //    ""parent_id"": null,
        //    ""guild_id"": ""213514616"",
        //    ""permission_overwrites"": [],
        //    ""nsfw"": false 
        //    }
        //    ]";

        //    var response = new HttpResponseMessage()
        //    {
        //        Content = new StringContent(jsonFromDiscordAPI)
        //    };
        //    handler.SetupAnyRequest()
        //            .ReturnsAsync(response);

        //    DiscordService discord = new DiscordService(handler.CreateClientFactory(), null, null, null);

        //    // Act
        //    List<Channel>? channels = await discord.GetGuildChannels("fakeBotToken", "fakeServerId");

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(channels!.Count, Is.EqualTo(2));
        //        Assert.That(channels[0].Id == "1035111022");
        //        Assert.That(channels[0].Name == "Text Channels");
        //    }
        //    );
        //}


        //[Test]
        //public async Task ServerChannels_ShouldReturnListOfChannels()
        //{
        //    // Arrange
        //    var handler = new Mock<HttpMessageHandler>();
        //    _channelRepository = new ChannelRepository(_mockContext.Object);
        //    _serverRepository = new ServerRepository(_mockContext.Object);

        //    AccountController controller = new AccountController(null, null, null, _serverRepository, _channelRepository,null);
        //    controller.ControllerContext = new ControllerContext();

        //    // Act
        //    ActionResult result = (ActionResult)await controller.ServerChannels("hello");
        //    var something = result;
        //    // Arrange

        //}


        [Test]
        public void ServerEntryDbCheck_TryingToAddAlreadyExistingServer_ShouldNotAddDuplicate()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _serverRepository = new ServerRepository(_mockContext.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null) ;

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);


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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);


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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);


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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null) ;

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
            ""Username"": ""test"",
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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

            // Act
            ServerOwnerViewModel? serverInfo = await discord.GetFullGuild("fakeBotToken", "fakeServerId");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(serverInfo.Id == "1035111022");
                Assert.That(serverInfo.Name == "testServer");
                Assert.That(serverInfo.Icon == "8342729096ea3675442027381ff50dfe");
                Assert.That(serverInfo.Approximate_Member_Count == 2);
            }
            );

        }

        [Test]
        public void GetGuildsInfo_WhereBotIsInServer__ShouldReturnTrue()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK);


            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, null,null);

            var a = discord.CheckForBot("FakeBotToken", "FakeServerId").Result;


            Assert.AreEqual(discord.CheckForBot("FakeBotToken", "FakeServerId").Result, "true");
        }

        [Test]
        public void GetGuildsInfo_WhereBotIsNotInServer__ShouldReturnFalse()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

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

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null,null);

            // Act
            string? responseInfo = await discord.AddMemberToGuild("fakeBotToken", "fakeServerId");
            var returnAnswer = addedMemberProcessInfoVM.infoOfProcessOfBeingAdded(responseInfo);
            var realAnswer = "You've already joined. From discord:  \r\n        \"joined_at\": \"2022-01-04T19:41:39.926000+00:00\"";

            // Assert
            Assert.AreEqual(returnAnswer, realAnswer);
        }
        [Test]
        public async Task Check_For_Duplicate_Presence_Should_Add_New_Presence()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _presenceRepository = new PresenceRepository(_mockContext1.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null);

            var presenceGetAll = _presenceRepository.GetAll().ToList();
            int initialCount = presenceGetAll.Count();
            var presence = new Presence[]
            {
                new Presence{Id = "38f6fcf02ade715a",  ApplicationId= "409416265891971072", Name = "RuneLite", Details = null, CreatedAt = now, LargeImageId="409453539270590464", SmallImageId=null, ServerId="836770827591876628", UserId="156209528969494528", Image="https://cdn.discordapp.com/app-assets/409416265891971072/409453539270590464.webp"},
            };

            // Act
            await discord.PresenceEntryAndUpdateDbCheck(presence); 
           
            int Updatedcount = _presenceRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(Updatedcount - 1, initialCount);
        }
        [Test]
        public async Task Check_For_Duplicate_Presence_With_Same_Names_Different_Servers_Should_Add_New_Presence()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _presenceRepository = new PresenceRepository(_mockContext1.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null);

            var presenceGetAll = _presenceRepository.GetAll().ToList();
            int initialCount = presenceGetAll.Count();
            var presence = new Presence[]
            {
                new Presence{Id = "38f6fcf02ade715a",  ApplicationId= "643897785271189524", Name = "Red Dead Redemption 2", Details = null, CreatedAt = now, LargeImageId="409453539270590464", SmallImageId=null, ServerId="416485574631751690", UserId="156209528969494528", Image="https://cdn.discordapp.com/app-assets/409416265891971072/409453539270590464.webp"},
            };

            // Act
            await discord.PresenceEntryAndUpdateDbCheck(presence);

            int Updatedcount = _presenceRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(Updatedcount - 1, initialCount);
        }
        [Test]
        public async Task Check_For_Duplicate_Presence_With_Different_Names_Same_Server_Should_Add_New_Presence()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _presenceRepository = new PresenceRepository(_mockContext1.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null);

            var presenceGetAll = _presenceRepository.GetAll().ToList();
            int initialCount = presenceGetAll.Count();
            var presence = new Presence[]
            {
                new Presence{Id = "84c1af280d1fb445",  ApplicationId= "451545729253638144", Name = "Fallout 3", Details = null, CreatedAt = now, LargeImageId="409453539270590464", SmallImageId=null, ServerId="416485574631751690", UserId="156209528969494528", Image="https://cdn.discordapp.com/app-assets/409416265891971072/409453539270590464.webp"},
            };

            // Act
            await discord.PresenceEntryAndUpdateDbCheck(presence);

            int Updatedcount = _presenceRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(Updatedcount - 1, initialCount);
        }
        [Test]
        public async Task Check_For_Duplicate_Presence_With_Same_Names_Same_Server_Should_Not_Add_New_Presence()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _presenceRepository = new PresenceRepository(_mockContext1.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, null);

            var presenceGetAll = _presenceRepository.GetAll().ToList();
            int initialCount = presenceGetAll.Count();
            var presence = new Presence[]
            {
                new Presence{Id = "789317480325316644",  ApplicationId= null, Name = "Calculator", Details = null, CreatedAt = now, LargeImageId=null, SmallImageId=null, ServerId="151515125129", UserId="41351461513545"},
            };

            // Act
            await discord.PresenceEntryAndUpdateDbCheck(presence);

            int Updatedcount = _presenceRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(Updatedcount, initialCount);
        }

        [Test]
        public async Task Check_For_Duplicate_VoiceChannel_With_Different_Counts_Same_Server_Should_Update_Count()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _voiceChannelRepository = new VoiceChannelRepository(_mockContext3.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, _voiceChannelRepository);

            var VoiceChannelGetAll = _voiceChannelRepository.GetAll().ToList();
            int initialCount = VoiceChannelGetAll.Count();
            var VoiceChannel = new VoiceChannel[]
            {
                new VoiceChannel{Id = "156209528969494528", Name ="General", Count=2,GuildId="928010025958510632",Time=DateTime.Parse("2022-04-06 20:17:38.263") }
           };

            // Act
            await discord.VoiceChannelEntryAndUpdateDbCheck(VoiceChannel);

           int Updatedcount = _voiceChannelRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(VoiceChannelGetAll[0].Count, 2);
        }

        [Test]
        public async Task Check_For_Duplicate_VoiceChannel_With_Same_Counts_Different_Hour_Should_Add_VoiceChannel()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _voiceChannelRepository = new VoiceChannelRepository(_mockContext3.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, _voiceChannelRepository);

            var VoiceChannelGetAll = _voiceChannelRepository.GetAll().ToList();
            int initialCount = VoiceChannelGetAll.Count();
            var VoiceChannel = new VoiceChannel[]
            {
                new VoiceChannel{Id = "156209528969494528", Name ="General", Count=2,GuildId="928010025958510632",Time=DateTime.Parse("2022-04-06 23:17:38.263") }
           };

            // Act
            await discord.VoiceChannelEntryAndUpdateDbCheck(VoiceChannel);

            int Updatedcount = _voiceChannelRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(initialCount, Updatedcount-1);
        }
        [Test]
        public async Task Check_For_Duplicate_VoiceChannel_With_Same_Counts_Same_Hour_Different_Day_Should_Add_VoiceChannel()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _voiceChannelRepository = new VoiceChannelRepository(_mockContext3.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, _voiceChannelRepository);

            var VoiceChannelGetAll = _voiceChannelRepository.GetAll().ToList();
            int initialCount = VoiceChannelGetAll.Count();
            var VoiceChannel = new VoiceChannel[]
            {
                new VoiceChannel{Id = "156209528969494528", Name ="General", Count=2,GuildId="928010025958510632",Time=DateTime.Parse("2022-04-07 20:17:38.263") }
           };

            // Act
            await discord.VoiceChannelEntryAndUpdateDbCheck(VoiceChannel);

            int Updatedcount = _voiceChannelRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(initialCount, Updatedcount - 1);
        }
        [Test]
        public async Task Check_For_Duplicate_VoiceChannel_Same_Record_Should_Not_Add()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            _voiceChannelRepository = new VoiceChannelRepository(_mockContext3.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, _presenceRepository, null, _voiceChannelRepository);

            var VoiceChannelGetAll = _voiceChannelRepository.GetAll().ToList();
            int initialCount = VoiceChannelGetAll.Count();
            var VoiceChannel = new VoiceChannel[]
            {
                new VoiceChannel{Id = "156209528969494528", Name ="General", Count=1,GuildId="928010025958510632",Time=DateTime.Parse("2022-04-06 20:27:38.263") }
           };

            // Act
            await discord.VoiceChannelEntryAndUpdateDbCheck(VoiceChannel);

            int Updatedcount = _voiceChannelRepository.GetAll().ToList().Count();


            // Assert
            Assert.AreEqual(initialCount, Updatedcount);
        }
    }
}
