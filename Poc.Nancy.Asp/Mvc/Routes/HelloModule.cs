using Nancy;

namespace ASaed.POC.Nancy.Asp.Mvc.Routes
{
    public class HelloModule : NancyModule
    {
        
        public HelloModule()
        {
            Get["/"] = parameters => "Hello World! I'm running as ASP.NET in IIS";
        }
        

        /*
        public HelloModule(IRootPathProvider rootPathProvider)
        {
            Get["/"] = parameters => string.Format("Hello World! I'm running as ASP.NET in IIS. My root path is {0}",
                rootPathProvider.GetRootPath());
        }
        */ 
    }
}