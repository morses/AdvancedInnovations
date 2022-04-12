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
    internal class AD_208_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<Presence>> _mockPresenceDbSet;

        private IServerRepository _serverRepository;

        private Mock<DbSet<ServerPartial>> _mockPartialServerDbSet;

        DateTime now = DateTime.Now;
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
            var ser = new List<Presence>
            {
                new Presence{Id="kjn235dgs8", ApplicationId="w4fc6", Name="Test Name", Details="4w5g45g", CreatedAt=now, LargeImageId="1252wfg24fg23f4v", SmallImageId="kljm4tkjl23465", ServerId="kjn253nbkjh", UserId="drg43wv343g4f", Image="rf43cv3f3"},
                new Presence{Id="w456h", ApplicationId="awe5yh4w", Name="j5w6hj", Details="srtghr5", CreatedAt=now, LargeImageId="w456ujwr546jh", SmallImageId="w54e6j5ew6j", ServerId="e5w6je56j", UserId="w456j5we6j", Image="w46jw456jsef"},
                new Presence{Id="134dx52f6", ApplicationId="qec6ty4e", Name="Test2 Name", Details="eh5r6b", CreatedAt=now, LargeImageId="wg45w4v5b", SmallImageId="qe45gaerg", ServerId="ghqe5gqew4", UserId="aerg456hg", Image="ghea4rgae4gf"},
                new Presence{Id="hb35653ggf35", ApplicationId="aectxtq", Name="Test3 Name", Details="werh6", CreatedAt=now, LargeImageId="wgwrthe4q5hg", SmallImageId="gsedfagaeg", ServerId="sXQQ32D", UserId="adfg345q", Image="asdrgaerg"},
                new Presence{Id="2f3c46243vg24", ApplicationId="c1 35134", Name="Test4 Name", Details="w456h", CreatedAt=now, LargeImageId="2   3rq453g", SmallImageId="aedrgaerge", ServerId="GRDFATYG", UserId="adrghae45ae", Image="aergaerbeyha"},

            }.AsQueryable();

            _mockPresenceDbSet = GetMockDbSet<Presence>(ser.AsQueryable<Presence>());
            var q = _mockPresenceDbSet.As<IQueryable<Presence>>();
            q.Setup(m => m.GetEnumerator()).Returns(() => ser.GetEnumerator());
            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.Presences).Returns(_mockPresenceDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Presence>()).Returns(_mockPresenceDbSet.Object);
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);


        }



        [Test]
        public void PresenceRepositoryGetAllReturnsCorrectNumberElements()
        {
            // Arrange
            IPresenceRepository PresenceRepo = new PresenceRepository(_mockContext.Object);


            // Act
            var allPresences = PresenceRepo.GetAll();


            // Assert that allPresences contain correct number of elements
            Assert.AreEqual(allPresences.Count(), 5);

        }

        [Test]
        public void PresenceRepositoryGetAllReturnsCorrectDataTypesForEachValue()
        {
            // Arrange
            IPresenceRepository PresenceRepo = new PresenceRepository(_mockContext.Object);


            // Act
            var allPresences = PresenceRepo.GetAll();



            // Assert that allPresences contain correct data types for each element

            foreach (Presence md in allPresences)
            {
                Assert.True(md.Id.GetType() == typeof(string));
                Assert.True(md.ApplicationId.GetType() == typeof(string));
                Assert.True(md.Name.GetType() == typeof(string));
                Assert.True(md.Details.GetType() == typeof(string));
                Assert.True(md.CreatedAt.GetType() == typeof(DateTime));
                Assert.True(md.LargeImageId.GetType() == typeof(string));
                Assert.True(md.SmallImageId.GetType() == typeof(string));
                Assert.True(md.ServerId.GetType() == typeof(string));
                Assert.True(md.UserId.GetType() == typeof(string));
                Assert.True(md.Image.GetType() == typeof(string));

            }

        }

        [Test]
        public void PresenceRepositoryGetAllReturnsCorrectDataValues()
        {
            var ser = new List<Presence>
            {
                new Presence{Id="kjn235dgs8", ApplicationId="w4fc6", Name="Test Name", Details="4w5g45g", CreatedAt=now, LargeImageId="1252wfg24fg23f4v", SmallImageId="kljm4tkjl23465", ServerId="kjn253nbkjh", UserId="drg43wv343g4f", Image="rf43cv3f3"},
                new Presence{Id="w456h", ApplicationId="awe5yh4w", Name="j5w6hj", Details="srtghr5", CreatedAt=now, LargeImageId="w456ujwr546jh", SmallImageId="w54e6j5ew6j", ServerId="e5w6je56j", UserId="w456j5we6j", Image="w46jw456jsef"},
                new Presence{Id="134dx52f6", ApplicationId="qec6ty4e", Name="Test2 Name", Details="eh5r6b", CreatedAt=now, LargeImageId="wg45w4v5b", SmallImageId="qe45gaerg", ServerId="ghqe5gqew4", UserId="aerg456hg", Image="ghea4rgae4gf"},
                new Presence{Id="hb35653ggf35", ApplicationId="aectxtq", Name="Test3 Name", Details="werh6", CreatedAt=now, LargeImageId="wgwrthe4q5hg", SmallImageId="gsedfagaeg", ServerId="sXQQ32D", UserId="adfg345q", Image="asdrgaerg"},
                new Presence{Id="2f3c46243vg24", ApplicationId="c1 35134", Name="Test4 Name", Details="w456h", CreatedAt=now, LargeImageId="2   3rq453g", SmallImageId="aedrgaerge", ServerId="GRDFATYG", UserId="adrghae45ae", Image="aergaerbeyha"}
,

            };
            // Arrange
            IPresenceRepository PresenceRepo = new PresenceRepository(_mockContext.Object);

            // Act
            var allPresences = PresenceRepo.GetAll().ToList();

            // Assert that allPresences contain correct values for each element
            for (int i = 0; i < allPresences.Count; i++)
            {
                Assert.AreEqual(allPresences[i].Id, ser[i].Id);
                Assert.AreEqual(allPresences[i].ApplicationId, ser[i].ApplicationId);
                Assert.AreEqual(allPresences[i].Name, ser[i].Name);
                Assert.AreEqual(allPresences[i].Details, ser[i].Details);
                Assert.AreEqual(allPresences[i].CreatedAt, ser[i].CreatedAt);
                Assert.AreEqual(allPresences[i].LargeImageId, ser[i].LargeImageId);
                Assert.AreEqual(allPresences[i].SmallImageId, ser[i].SmallImageId);
                Assert.AreEqual(allPresences[i].ServerId, ser[i].ServerId);
                Assert.AreEqual(allPresences[i].UserId, ser[i].UserId);
                Assert.AreEqual(allPresences[i].Image, ser[i].Image);
            }
        }

        [Test]
        public async Task ApiControllerPresenceGetRequestReturnsCorrectNumberElements()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("Stats/GetPresenceFromDatabase?serverid=hsjv26512", Method.Get);

            // act
            var response = await client.GetAsync<List<Presence>>(request);

            // assert that all of the data received is the correct number of elements
            Assert.AreEqual(response.Count, 5);
        }

        [Test]
        public async Task ApiControllerPresenceGetRequestReturnsCorrectDataTypesForEachValue()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("Stats/GetPresenceFromDatabase?serverid=hsjv26512");


            // act
            //var response = await client.GetAsync<List<Presence>>(request);
            var response = await client.ExecuteGetAsync(request);
            var PresenceList = JsonSerializer.Deserialize<List<Presence>>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            // Assert that allPresences contain correct data types for each element

            foreach (Presence md in PresenceList)
            {
                Assert.True(md.Id.GetType() == null || md.Id.GetType() == typeof(string));
                Assert.True(md.ApplicationId.GetType() == null || md.ApplicationId.GetType() == typeof(string));
                Assert.True(md.Name.GetType() == null || md.Name.GetType() == typeof(string));
                Assert.True(md.Details.GetType() == null || md.Details.GetType() == typeof(string));
                Assert.True(md.CreatedAt.GetType() == null || md.CreatedAt.GetType() == typeof(DateTime));
                Assert.True(md.LargeImageId.GetType() == null || md.LargeImageId.GetType() == typeof(string));
                Assert.True(md.SmallImageId.GetType() == null || md.SmallImageId.GetType() == typeof(string));
                Assert.True(md.ServerId.GetType() == null || md.ServerId.GetType() == typeof(string));
                Assert.True(md.UserId.GetType() == null || md.UserId.GetType() == typeof(string));
                Assert.True(md.Image.GetType() == null || md.Image.GetType() == typeof(string));

            }

        }

        [Test]
        public async Task ApiControllerPresenceGetRequestReturnsCorrectDataValues()
        {
            // arrange
            var client = new RestClient("https://localhost:7228/");
            var request = new RestRequest("Stats/GetPresenceFromDatabase?serverid=hsjv26512");


            // act
            //var response = await client.GetAsync<List<Presence>>(request);
            var response = await client.ExecuteGetAsync(request);
            var PresenceList = JsonSerializer.Deserialize<List<Presence>>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var ser = new List<Presence>
            {
                new Presence{Id="kjn235dgs8", ApplicationId="w4fc6", Name="Test Name", Details="4w5g45g", CreatedAt=now, LargeImageId="1252wfg24fg23f4v", SmallImageId="kljm4tkjl23465", ServerId="kjn253nbkjh", UserId="drg43wv343g4f", Image="rf43cv3f3"},
                new Presence{Id="w456h", ApplicationId="awe5yh4w", Name="j5w6hj", Details="srtghr5", CreatedAt=now, LargeImageId="w456ujwr546jh", SmallImageId="w54e6j5ew6j", ServerId="e5w6je56j", UserId="w456j5we6j", Image="w46jw456jsef"},
                new Presence{Id="134dx52f6", ApplicationId="qec6ty4e", Name="Test2 Name", Details="eh5r6b", CreatedAt=now, LargeImageId="wg45w4v5b", SmallImageId="qe45gaerg", ServerId="ghqe5gqew4", UserId="aerg456hg", Image="ghea4rgae4gf"},
                new Presence{Id="hb35653ggf35", ApplicationId="aectxtq", Name="Test3 Name", Details="werh6", CreatedAt=now, LargeImageId="wgwrthe4q5hg", SmallImageId="gsedfagaeg", ServerId="sXQQ32D", UserId="adfg345q", Image="asdrgaerg"},
                new Presence{Id="2f3c46243vg24", ApplicationId="c1 35134", Name="Test4 Name", Details="w456h", CreatedAt=now, LargeImageId="2   3rq453g", SmallImageId="aedrgaerge", ServerId="GRDFATYG", UserId="adrghae45ae", Image="aergaerbeyha"},


            };


            // Assert that allPresences contain correct values for each element

            for (int i = 0; i < PresenceList.Count; i++)
            {
                Assert.AreEqual(PresenceList[i].Id, ser[i].Id);
                Assert.AreEqual(PresenceList[i].ApplicationId, ser[i].ApplicationId);
                Assert.AreEqual(PresenceList[i].Name, ser[i].Name);
                Assert.AreEqual(PresenceList[i].Details, ser[i].Details);
                Assert.AreEqual(PresenceList[i].CreatedAt, ser[i].CreatedAt);
                Assert.AreEqual(PresenceList[i].LargeImageId, ser[i].LargeImageId);
                Assert.AreEqual(PresenceList[i].SmallImageId, ser[i].SmallImageId);
                Assert.AreEqual(PresenceList[i].ServerId, ser[i].ServerId);
                Assert.AreEqual(PresenceList[i].UserId, ser[i].UserId);
                Assert.AreEqual(PresenceList[i].Image, ser[i].Image);
            }

        }
    }
}
