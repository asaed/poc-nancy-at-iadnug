using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
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

namespace Poc.Nancy.SelfHost.Tests.bTestAuthorViews
{
    [TestFixture]
    public class AuthorViewModuleTest
    {
        // #anId
        // .aClass
        private Browser browser;
        private Mock<IAuthorRepository> mockRepo;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IAuthorRepository>();

            browser = new Browser(boot =>
            {
                boot.Module<AuthorViewModule>();
                boot.Dependency<IAuthorRepository>(mockRepo.Object);
                
                //talk about me later 
                boot.ViewFactory<TestingViewFactory>();
            }, defaults =>
            {
                defaults.Accept("text/html");
            });

        }

        [Test]
        public void TestListOfAuthorsView()
        {
            mockRepo.Setup(r => r.GetAll()).Returns(new List<Author> { new Author { 
                Id = 6,
                Name = "Sidney Sheldon",
                NumberOfPublications = 30
            } });

            var response = browser.Get("/authors/all");

            Console.WriteLine("----------------- START");
            Console.WriteLine(response.Body.AsString());
            Console.WriteLine("----------------- END");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html", response.ContentType);
            Assert.IsTrue(response.Body.AsString().StartsWith("<!DOCTYPE html>"));
        }

        [Test]
        public void TestListOfAuthorsViewWithSelectors_NoAuthors()
        {
            mockRepo.Setup(r => r.GetAll()).Returns(new List<Author> {});

            var response = browser.Get("/authors/all");

            //Console.WriteLine("----------------- START");
            //Console.WriteLine(response.Body.AsString());
            //Console.WriteLine("----------------- END");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html", response.ContentType);
            
            response.Body["#noAuthorsMsg"]
                .ShouldExistOnce()
                .And
                .ShouldContain("Sorry, no authors were found");
            
            response.Body["#authorList"].ShouldNotExist();

        }

        [Test]
        public void TestListOfAuthorsViewWithSelectors()
        {
            mockRepo.Setup(r => r.GetAll()).Returns(new List<Author> { new Author { 
                Id = 6,
                Name = "Sidney Sheldon",
                NumberOfPublications = 30
            } });

            var response = browser.Get("/authors/all");

            //Console.WriteLine("----------------- START");
            //Console.WriteLine(response.Body.AsString());
            //Console.WriteLine("----------------- END");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html", response.ContentType);

            response.Body["#authorList"].ShouldExistOnce();

            response.Body["#authorList tr"].ShouldExistExactly(2);

            response.Body["#authorList tr"]["th"].ShouldExistExactly(3);
            response.Body["#authorList tr"]["th"].ToList()[0].ShouldContain("Id");
            response.Body["#authorList tr"]["th"].ToList()[1].ShouldContain("Author Name");
            response.Body["#authorList tr"]["th"].ToList()[2].ShouldContain("No. of Publications");


            response.Body["#authorList tr"]["td"].ShouldExistExactly(3);
            response.Body["#authorList tr"]["td"].ToList()[0].ShouldContain("6");
            response.Body["#authorList tr"]["td"].ToList()[1].ShouldContain("Sidney Sheldon");
            response.Body["#authorList tr"]["td"].ToList()[2].ShouldContain("30");

            response.Body["#noAuthorsMsg"].ShouldNotExist();

        }


        [Test]
        public void TestListOfAuthorsViewWithViewFactory()
        {
            var expectedAuthors = new List<Author> { new Author { 
                Id = 6,
                Name = "Sidney Sheldon",
                NumberOfPublications = 30
            } };
            mockRepo.Setup(r => r.GetAll()).Returns(expectedAuthors);

            var response = browser.Get("/authors/all");

            //Console.WriteLine("----------------- START");
            //Console.WriteLine(response.Body.AsString());
            //Console.WriteLine("----------------- END");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("authors/all", response.GetViewName());

            ExpandoObject expandoModel = response.GetModel<ExpandoObject>();
            var actualModel = (IDictionary<string, object>)expandoModel;

            Assert.AreEqual(true, actualModel["HasAuthors"]);
            Assert.AreEqual(expectedAuthors, actualModel["Authors"]);
        }

        [Test]
        public void TestPostingAuthorWithFormSubmission()
        {
            var expectedAuthors = new List<Author> { new Author { 
                Id = 6,
                Name = "Sidney Sheldon",
                NumberOfPublications = 30
            } };
            Author actualSavedAuthor = null;
            mockRepo.Setup(r => r.GetAll()).Returns(expectedAuthors);
            mockRepo.Setup(r => r.Save(It.IsAny<Author>())).Returns(17)
                .Callback<Author>(a => actualSavedAuthor = a);

            var response = browser.Post("/authors/addAuthor", (with) => {
                with.FormValue("name", "Martin Smith");
                with.FormValue("NumberOfPublications", "5");
            });



            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("authors/addAuthor", response.GetViewName());

            mockRepo.Verify(r => r.Save(It.IsAny<Author>()), Times.Once);
            Assert.AreEqual(0, actualSavedAuthor.Id);
            Assert.AreEqual("Martin Smith", actualSavedAuthor.Name);
            Assert.AreEqual(5, actualSavedAuthor.NumberOfPublications);

            ExpandoObject expandoModel = response.GetModel<ExpandoObject>();
            var actualModel = (IDictionary<string, object>)expandoModel;

            Assert.AreEqual(true, actualModel["HasMessage"]);
            Assert.AreEqual("Author was saved with Id 17", actualModel["Message"]);
            Assert.AreEqual(true, actualModel["HasAuthors"]);
            Assert.AreEqual(expectedAuthors, actualModel["Authors"]);
        }
    }
}
