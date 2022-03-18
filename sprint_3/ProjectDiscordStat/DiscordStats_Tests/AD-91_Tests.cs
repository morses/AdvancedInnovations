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
using RestSharp;

namespace DiscordStats_Tests
{
    internal class AD_91_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private List<Server> _servers = FakeData.Servers;

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



        [Test]
        public void MessageDataRepositoryGetAllReturnsCorrectNumberElements()
        {
            // Arrange
            IMessageDataRepository messageDataRepo = new MessageDataRepository(_mockContext.Object);


            // Act
            var allMessageDatas = messageDataRepo.GetAll();


            // Assert that allMessageDatas contain correct number of elements
            Assert.AreEqual(allMessageDatas.Count, #CorrectNumberWeAreComparingWith)

        }

        [Test]
        public void MessageDataRepositoryGetAllReturnsCorrectDataTypesForEachValue()
        {
            // Arrange
            IMessageDataRepository messageDataRepo = new MessageDataRepository(_mockContext.Object);


            // Act
            var allMessageDatas = messageDataRepo.GetAll();



            // Assert that allMessageDatas contain correct data types for each element

            foreach (MessageData md in allMessageDatas)
            {
                Assert.AreEqual(md.ChannelId.GetType(), typeof(#ExpectedType));
                Assert.AreEqual(md.ServerId.GetType(), typeof(#ExpectedType));
                Assert.AreEqual(md.UserId.GetType(), typeof(#ExpectedType));
                Assert.AreEqual(md.DataTimeSent.GetType(), typeof(#ExpectedType));
            }

        }

        [Test]
        public void MessageDataRepositoryGetAllReturnsCorrectDataValues()
        {
            // Arrange
            IMessageDataRepository messageDataRepo = new MessageDataRepository(_mockContext.Object);

            // Act
            var allMessageDatas = messageDataRepo.GetAll();

            // Assert that allMessageDatas contain correct values for each element
            foreach (MessageData md in allMessageDatas)
            {
                Assert.AreEqual(md.ChannelId, #ExpectedValue);
                Assert.AreEqual(md.ServerId, #ExpectedValue);
                Assert.AreEqual(md.UserId, #ExpectedValue)
                Assert.AreEqual(md.DataTimeSent, #ExpectedValue);
            }
        }

        [Test]
        public async Task ApiControllerMessageDataGetRequestReturnsCorrectNumberElements()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("api/GetMessageData");

            // act
            var response = await client.GetAsync<List<MessageData>>(request);

            // assert that all of the data received is the correct number of elements
            Assert.AreEqual(response.Count, #CorrectNumberWeAreComparingWith)
        }

        [Test]
        public async Task ApiControllerMessageDataGetRequestReturnsCorrectDataTypesForEachValue()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("api/GetMessageData");

            // act
            var response = await client.GetAsync<List<MessageData>>(request);



            // Assert that allMessageDatas contain correct data types for each element

            foreach (MessageData md in response)
            {
                Assert.AreEqual(md.ChannelId.GetType(), typeof(#ExpectedType));
                Assert.AreEqual(md.ServerId.GetType(), typeof(#ExpectedType));
                Assert.AreEqual(md.UserId.GetType(), typeof(#ExpectedType));
                Assert.AreEqual(md.DataTimeSent.GetType(), typeof(#ExpectedType));
            }

        }

        [Test]
        public async Task ApiControllerMessageDataGetRequestReturnsCorrectDataValues()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("api/GetMessageData");

            // act
            var response = await client.GetAsync<List<MessageData>>(request);



            // Assert that allMessageDatas contain correct values for each element

            foreach (MessageData md in allMessageDatas)
            {
                Assert.AreEqual(md.ChannelId, #ExpectedValue);
                Assert.AreEqual(md.ServerId, #ExpectedValue);
                Assert.AreEqual(md.UserId, #ExpectedValue)
                Assert.AreEqual(md.DataTimeSent, #ExpectedValue);
            }

        }
    }
}
