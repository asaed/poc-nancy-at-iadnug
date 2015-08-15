using System;
using System.Dynamic;
using Nancy;

namespace ASaed.POC.Nancy.SelfHost
{
    public class HelloModule : NancyModule
    {
        
        public HelloModule()
        {
            Get["/"] = _ => "Hello World! I'm running from console as Self Hosted";
        }
        

        /*
        public HelloModule(IRootPathProvider rootPathProvider)
        {
            Get["/"] = parameters => string.Format("Hello World! I'm running from console as Self Hosted. My root path is {0}",
                rootPathProvider.GetRootPath());
        }
        */

    }

}