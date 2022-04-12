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

namespace DiscordStats_Tests
{
    public class DiscordStats_RemoveUser
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private List<Server> _servers = FakeData.Servers;

        private IServerRepository _serverRepository;
        private IDiscordService discordService;

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
            _mockServerDbSet = GetMockDbSet(_servers.AsQueryable());

            // _mockPartialServerDbSet = GetMockDbSet(_serverPartial.AsQueryable());


            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);

            //how do I do the ServerPartial?
            //_mockContext.Setup(ctx => ctx.ServersPartial).Returns(_mockServerDbSet.Object);
            //_mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
        }



        [Test]
        public async Task RemoveUser_User_Not_In_Server()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                        .ReturnsResponse(HttpStatusCode.NotFound);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, null,null);
            Task<string?> Act() => discord.RemoveUserServer("FakeBot", "11111", "thisUser");
            Assert.That(Act, Throws.TypeOf<HttpRequestException>());
        }

        [Test]
        public async Task RemoveUser_User_Is_in_Server()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.SetupAnyRequest()
                        .ReturnsResponse(HttpStatusCode.OK);

            DiscordService discord = new DiscordService(handler.CreateClientFactory(), _serverRepository, null, null,null);
            var result = discord.CheckForBot("FakeBotToken", "FakeServerId").Result;
            Assert.AreEqual(result, "true");
        }



    }
}