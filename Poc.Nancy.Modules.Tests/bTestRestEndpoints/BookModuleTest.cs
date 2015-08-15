using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Testing;
using NUnit;
using NUnit.Framework;
using Moq;
using ASaed.POC.Nancy.Modules;
using ASaed.POC.Nancy.Modules.Data;
using System.Dynamic;
using ASaed.POC.Nancy.Modules.Data.Model;
using ASaed.POC.Nancy.Modules.Data.Repositories;
using ASaed.POC.Nancy.Modules.Mvc.Routes;

namespace Poc.Nancy.Modules.Tests.bTestRestEndpoints
{
    [TestFixture]
    public class BookModuleTest
    {
        private Browser browser;
        private Mock<IBookRepository> mockRepo;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IBookRepository>();

            browser = new Browser(boot =>
            {
                boot.Module<BookRestModule>();
                boot.Dependency<IBookRepository>(mockRepo.Object);
            }, defaults => {
                defaults.Accept("application/json");
                defaults.Header("Content-Type", "application/json");
            });
        }

        [Test]
        public void EmptyListOfBooks()
        {
            mockRepo.Setup(r => r.GetAll()).Returns(new List<Book> { });

            var response = browser.Get("/books");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Body.ContentType);
            Assert.AreEqual("[]", response.Body.AsString());
        }

        [Test]
        public void NonEmptyListOfBooks()
        {
            mockRepo.Setup(r => r.GetAll()).Returns(new List<Book> {
            new Book {
                Id=56,
                AuthorName= "Sidney Sheldon",
                Title="The Doomsday Conspiracy",
                PublicationYear = 1991
            }});

            var response = browser.Get("/books");

            Console.WriteLine(response.Body.AsString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Body.ContentType);

            var expandos = response.Body.DeserializeJson<List<Book>>();
            Assert.AreEqual(1, expandos.Count);
            Assert.AreEqual(56, expandos[0].Id);
            Assert.AreEqual("Sidney Sheldon", expandos[0].AuthorName);
            Assert.AreEqual("The Doomsday Conspiracy", expandos[0].Title);
            Assert.AreEqual(1991, expandos[0].PublicationYear);
            
        }

        [Test]
        public void TestSearchWithoutAuthorName()
        {
            mockRepo.Setup(r => r.GetByAuthorName(It.IsAny<string>()))
                .Returns(new List<Book> { });

            var response = browser.Get("/books/search");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("You didn't pass an author name", response.Body.AsString());
            mockRepo.Verify(r => r.GetByAuthorName(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TestSearchWithAuthorName()
        {

            var response = browser.Get("/books/search/martin%20smith");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("", response.Body.AsString());
            mockRepo.Verify(r => r.GetByAuthorName("martin smith"), Times.Once);
        }

        [Test]
        public void TestSearchQueryWithAuthorName()
        {

            var response = browser.Get("/books/searchQuery", (with) => {
                with.Query("author", "martin smith");
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("", response.Body.AsString());
            mockRepo.Verify(r => r.GetByAuthorName("martin smith"), Times.Once);
        }

        [Test]
        public void TestPostWithEmptyBody()
        {
            mockRepo.Setup(r => r.Save(It.IsAny<Book>())).Returns((long?) null);

            var response = browser.Post("/books/best", (with) => {
                with.Body("{}");
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void TestPostWithBookInBody()
        {
            Book actualBook = null;
            mockRepo.Setup(r => r.Save(It.IsAny<Book>())).Returns(5)
                .Callback<Book>((book) => actualBook = book);

            var response = browser.Post("/books/best", (with) =>
            {
                with.Body("{\"authorName\":\"Sidney\",\"title\":\"Doomsday\"}");
            });

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("/books/5", response.Headers["Location"]);
            Assert.IsNotNull(actualBook);
            Assert.AreEqual(0, actualBook.Id);
            Assert.AreEqual("Sidney", actualBook.AuthorName);
            Assert.AreEqual("Doomsday", actualBook.Title);
            Assert.AreEqual(0, actualBook.PublicationYear);
        }

        /*
        [Test]
        public void TestPostWithBookInBodyAndIgnoreId()
        {
            Book actualBook = null;
            mockRepo.Setup(r => r.Save(It.IsAny<Book>())).Returns(5)
                .Callback<Book>((book) => actualBook = book);

            var response = browser.Post("/books/best", (with) =>
            {
                with.Body("{\"id\":18,\"authorName\":\"Sidney\",\"title\":\"Doomsday\"}");
            });

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("/books/5", response.Headers["Location"]);
            Assert.IsNotNull(actualBook);
            Assert.AreEqual(0, actualBook.Id);
            Assert.AreEqual("Sidney", actualBook.AuthorName);
            Assert.AreEqual("Doomsday", actualBook.Title);
            Assert.AreEqual(0, actualBook.PublicationYear);
        }
        */
    }
}
