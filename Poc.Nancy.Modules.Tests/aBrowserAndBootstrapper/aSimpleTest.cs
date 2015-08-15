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
using ASaed.POC.Nancy.Modules.Data.Model;
using ASaed.POC.Nancy.Modules.Data.Repositories;
using ASaed.POC.Nancy.Modules.Mvc.Routes;

namespace Poc.Nancy.Modules.Tests.aBrowserAndBootstrapper
{
    [TestFixture]
    public class aSimpleTest
    {
        [Test]
        public void LoadAllModulesAndTestThem()
        {
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            BrowserResponse response = browser.Get("/books", (with) => {
                with.HttpRequest();
                with.Accept("application/json");
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Body.ContentType);
            Assert.IsTrue(response.Body.AsString().Length > 0);
            Console.WriteLine(response.Body.AsString());
        }








        [Test]
        public void LoadOneModuleOnlyAndTestIt()
        {
            var browser = new Browser(with => {
                with.Module<BookRestModule>();
                with.EnableAutoRegistration();
            });

            BrowserResponse response = browser.Get("/books", (with) =>
            {
                with.HttpRequest();
                with.Accept("application/json");
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Body.ContentType);
            Assert.IsTrue(response.Body.AsString().Length > 0);
            Console.WriteLine(response.Body.AsString());
        }






        [Test]
        public void LoadOneModuleAndStubItsDependencies()
        {
            var mockBookRepo = new Mock<IBookRepository>();
            mockBookRepo.Setup(r => r.GetAll()).Returns(new List<Book>{new Book()});

            var browser = new Browser(with =>
            {
                with.Module<BookRestModule>();
                with.Dependency<IBookRepository>(mockBookRepo.Object);
            });

            BrowserResponse response = browser.Get("/books", (with) =>
            {
                with.HttpRequest();
                with.Accept("application/json");
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Body.ContentType);
            Assert.AreEqual("[{\"id\":0,\"authorName\":null,\"title\":null,\"publicationYear\":0}]",
                response.Body.AsString());
        }
    }
}
