using System.Dynamic;
using Nancy;

namespace ASaed.POC.Nancy.Modules.Mvc.Routes
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            //Return simple View with a model 
            Get["/home"] = _ =>
            {
                dynamic model = new ExpandoObject();
                model.Message = "Hello World! This is a POC for NancyFx";
                
                return View["home", model];
            };

            //static view with master layout
            Get["/homeWithMaster"] = _ => View["homeWithMaster"];

            //static view with CSS in master layout
            Get["/homeWithStylishMaster"] = _ => View["homeWithStylishMaster"];

        }
    }

}