using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
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
using RestSharp;
using System.Text.Json;

namespace DiscordStats_Tests
{
    internal class AD_91_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<MessageInfo>> _mockMessageInfoDbSet;

        private IServerRepository _serverRepository;

        private Mock<DbSet<ServerPartial>> _mockPartialServerDbSet;
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
            var ser = new List<MessageInfo>
            {
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "ghjh234b3", UserId = "njk326j2326lkn1q", CreatedAt = "10"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "jhv8khgv42", UserId = "jkhg27khjv3q", CreatedAt = "12"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "456fgh786dfg", UserId = "y67dfdg685dg", CreatedAt = "5"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "7685hdz", UserId = "65Zg5678SD", CreatedAt = "3"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "hfdzdr5g3refg34", UserId = "f34f3gqw45tfgt3aeqw", CreatedAt = "6"},

            }.AsQueryable();

            _mockMessageInfoDbSet = GetMockDbSet<MessageInfo>(ser.AsQueryable<MessageInfo>());
            var q = _mockMessageInfoDbSet.As<IQueryable<MessageInfo>>();
            q.Setup(m => m.GetEnumerator()).Returns(() => ser.GetEnumerator());
            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.MessageInfos).Returns(_mockMessageInfoDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<MessageInfo>()).Returns(_mockMessageInfoDbSet.Object);
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);


        }



        [Test]
        public void MessageInfoRepositoryGetAllReturnsCorrectNumberElements()
        {
            // Arrange
            IMessageInfoRepository MessageInfoRepo = new MessageInfoRepository(_mockContext.Object);


            // Act
            var allMessageInfos = MessageInfoRepo.GetAll();


            // Assert that allMessageInfos contain correct number of elements
            Assert.AreEqual(allMessageInfos.Count(), 5);

        }

        [Test]
        public void MessageInfoRepositoryGetAllReturnsCorrectDataTypesForEachValue()
        {
            // Arrange
            IMessageInfoRepository MessageInfoRepo = new MessageInfoRepository(_mockContext.Object);


            // Act
            var allMessageInfos = MessageInfoRepo.GetAll();



            // Assert that allMessageInfos contain correct data types for each element

            foreach (MessageInfo md in allMessageInfos)
            {
                Assert.AreEqual(md.ChannelId.GetType(), typeof(string));
                Assert.AreEqual(md.ServerId.GetType(), typeof(string));
                Assert.AreEqual(md.UserId.GetType(), typeof(string));
                Assert.AreEqual(md.CreatedAt.GetType(), typeof(string));
            }

        }

        [Test]
        public void MessageInfoRepositoryGetAllReturnsCorrectDataValues()
        {
            var ser = new List<MessageInfo>
            {
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "ghjh234b3", UserId = "njk326j2326lkn1q", CreatedAt = "10"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "jhv8khgv42", UserId = "jkhg27khjv3q", CreatedAt = "12"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "456fgh786dfg", UserId = "y67dfdg685dg", CreatedAt = "5"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "7685hdz", UserId = "65Zg5678SD", CreatedAt = "3"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "hfdzdr5g3refg34", UserId = "f34f3gqw45tfgt3aeqw", CreatedAt = "6"},

            };
            // Arrange
            IMessageInfoRepository MessageInfoRepo = new MessageInfoRepository(_mockContext.Object);

            // Act
            var allMessageInfos = MessageInfoRepo.GetAll().ToList();

            // Assert that allMessageInfos contain correct values for each element
            for (int i = 0; i < allMessageInfos.Count; i++)
            {
                Assert.AreEqual(allMessageInfos[i].ChannelId, ser[i].ChannelId);
                Assert.AreEqual(allMessageInfos[i].ServerId, ser[i].ServerId);
                Assert.AreEqual(allMessageInfos[i].UserId, ser[i].UserId);
                Assert.AreEqual(allMessageInfos[i].CreatedAt, ser[i].CreatedAt);
            }
        }

        [Test]
        public async Task ApiControllerMessageInfoGetRequestReturnsCorrectNumberElements()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("Stats/GetMessageInfoFromDatabase?serverid=hsjv26512", Method.Get);

            // act
            var response = await client.GetAsync<List<MessageInfo>>(request);

            // assert that all of the data received is the correct number of elements
            Assert.AreEqual(response.Count, 5);
        }

        [Test]
        public async Task ApiControllerMessageInfoGetRequestReturnsCorrectDataTypesForEachValue()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("Stats/GetMessageInfoFromDatabase?serverid=hsjv26512");


            // act
            //var response = await client.GetAsync<List<MessageInfo>>(request);
            var response = await client.ExecuteGetAsync(request);
            var messageInfoList = JsonSerializer.Deserialize<List<MessageInfo>>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            // Assert that allMessageInfos contain correct data types for each element

            foreach (MessageInfo md in messageInfoList)
            {
                Assert.AreEqual(md.ChannelId.GetType(), typeof(string));
                Assert.AreEqual(md.ServerId.GetType(), typeof(string));
                Assert.AreEqual(md.UserId.GetType(), typeof(string));
                Assert.AreEqual(md.CreatedAt.GetType(), typeof(string));
            }

        }

        [Test]
        public async Task ApiControllerMessageInfoGetRequestReturnsCorrectDataValues()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("Stats/GetMessageInfoFromDatabase?serverid=hsjv26512");


            // act
            //var response = await client.GetAsync<List<MessageInfo>>(request);
            var response = await client.ExecuteGetAsync(request);
            var messageInfoList = JsonSerializer.Deserialize<List<MessageInfo>>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var ser = new List<MessageInfo>
            {
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "ghjh234b3", UserId = "njk326j2326lkn1q", CreatedAt = "10"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "jhv8khgv42", UserId = "jkhg27khjv3q", CreatedAt = "12"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "456fgh786dfg", UserId = "y67dfdg685dg", CreatedAt = "5"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "7685hdz", UserId = "65Zg5678SD", CreatedAt = "3"},
                new MessageInfo{ServerId = "hsjv26512", ChannelId = "hfdzdr5g3refg34", UserId = "f34f3gqw45tfgt3aeqw", CreatedAt = "6"},

            };


            // Assert that allMessageInfos contain correct values for each element

            for (int i = 0; i < messageInfoList.Count; i++)
            {
                Assert.AreEqual(messageInfoList[i].ChannelId, ser[i].ChannelId);
                Assert.AreEqual(messageInfoList[i].ServerId, ser[i].ServerId);
                Assert.AreEqual(messageInfoList[i].UserId, ser[i].UserId);
                Assert.AreEqual(messageInfoList[i].CreatedAt, ser[i].CreatedAt);
            }

        }
    }
}
