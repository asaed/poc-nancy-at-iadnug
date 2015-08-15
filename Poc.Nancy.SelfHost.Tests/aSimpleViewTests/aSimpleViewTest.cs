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
using ASaed.POC.Nancy.Modules.Mvc.Routes;

namespace Poc.Nancy.SelfHost.Tests.aSimpleViewTests
{
    public class aSimpleViewTest
    {

        [Test]
        public void LoadAllModulesAndTestView()
        {
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            var response = browser.Get("/authors", (with) => {
                with.HttpRequest();
            });

            
            response.ShouldHaveRedirectedTo("/homeWithStylishMaster");
        }

        [Test]
        public void LoadAuthorModuleAndTestView()
        {
            
            var browser = new Browser(boot => {
                boot.Module<AuthorViewModule>();
                boot.EnableAutoRegistration();
            });

            var response = browser.Get("/authors", (with) =>
            {
                with.HttpRequest();
            });


            response.ShouldHaveRedirectedTo("/homeWithStylishMaster");
        }
    }
}
