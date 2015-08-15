using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit;
using NUnit.Framework;
using Moq;
using ASaed.POC.Nancy.Modules;
using ASaed.POC.Nancy.Modules.Data;
using ASaed.POC.Nancy.Modules.Data.Model;
using ASaed.POC.Nancy.Modules.Data.Repositories;
using ASaed.POC.Nancy.Modules.Mvc.Routes;

namespace Poc.Nancy.SelfHost.Tests.cTestContentNegotiation
{
    [TestFixture]
    public class GenreModuleTest
    {
        private Browser browser;
        private Mock<IGenreRepository> mockRepo;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IGenreRepository>();

            browser = new Browser(boot =>
            {
                boot.Module<GenreRestfulViewModule>();
                boot.Dependency<IGenreRepository>(mockRepo.Object);

                
                boot.ViewFactory<TestingViewFactory>();
            });
        }

        [Test]
        public void GetHtmlViewWhenNoAcceptHeader()
        {
            var expectedGenres = new List<Genre> { CreateGenre() };
            mockRepo.Setup(r => r.GetAll()).Returns(expectedGenres);

            var response = browser.Get("/genres");

            Assert.AreEqual("text/html", response.ContentType);
            Assert.AreEqual("genres/all", response.GetViewName());
            var actualModel = (IDictionary<string,object>) response.GetModel<ExpandoObject>();
            Assert.AreEqual(true, actualModel["HasGenres"]);
            Assert.AreSame(expectedGenres, actualModel["Genres"]);

        }

        [Test]
        public void GetHtmlViewWhenAcceptHeaderIsJson()
        {
            var expectedGenres = new List<Genre> { CreateGenre()};
            mockRepo.Setup(r => r.GetAll()).Returns(expectedGenres);

            var response = browser.Get("/genres", (with) => {
                with.Accept("application/json");
            });

            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("", response.GetViewName());
            
            var actualGenres = response.Body.DeserializeJson<List<Genre>>();
            Assert.AreEqual(1, actualGenres.Count);
            Assert.AreEqual(17, actualGenres[0].Id);
            Assert.AreEqual("Sci-Fi", actualGenres[0].Name);
            Assert.AreEqual(120, actualGenres[0].NumberOfAvailableTitles);
            
        }

        [Test]
        public void GetHtmlViewWhenAcceptHeaderIsXml()
        {
            var expectedGenres = new List<Genre> { CreateGenre() };
            mockRepo.Setup(r => r.GetAll()).Returns(expectedGenres);

            var response = browser.Get("/genres", (with) =>
            {
                with.Accept("application/xml");
            });

            Assert.AreEqual("application/xml", response.ContentType);
            Assert.AreEqual("", response.GetViewName());

            var actualGenres = response.Body.DeserializeXml<List<Genre>>();
            Assert.AreEqual(1, actualGenres.Count);
            Assert.AreEqual(17, actualGenres[0].Id);
            Assert.AreEqual("Sci-Fi", actualGenres[0].Name);
            Assert.AreEqual(120, actualGenres[0].NumberOfAvailableTitles);

        }

        /*
        [Test]
        public void GetHtmlViewWhenAcceptHeaderIsTextJson()
        {
            var expectedGenres = new List<Genre> { CreateGenre() };
            mockRepo.Setup(r => r.GetAll()).Returns(expectedGenres);

            var response = browser.Get("/genres", (with) =>
            {
                with.Accept("text/json");
            });

            
            Console.WriteLine(response.Body.AsString());
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("", response.GetViewName());

            var actualGenres = response.Body.DeserializeJson<List<Genre>>();
            Assert.AreEqual(1, actualGenres.Count);
            Assert.AreEqual(17, actualGenres[0].Id);
            Assert.AreEqual("Sci-Fi", actualGenres[0].Name);
            Assert.AreEqual(120, actualGenres[0].NumberOfAvailableTitles);

        }
        */
        private Genre CreateGenre()
        {
            return new Genre()
            {
                Id = 17,
                Name = "Sci-Fi",
                NumberOfAvailableTitles = 120
            };
        }
    }
}
