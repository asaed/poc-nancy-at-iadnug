using System;
using System.Linq;
using Nancy.Bootstrapper;

namespace ASaed.POC.Nancy.Modules.Mvc.Startup
{
    public class ForAllRequests : IApplicationStartup 
    {
        public void Initialize(IPipelines pipelines)
        {
            //Application-level interceptors (via application pipeline)
            pipelines.BeforeRequest += (ctx) =>
            {
                var abort = ctx.Request.Headers["X-Poc-Abort"].ToList();
                if (abort.Any() && abort.First() == "Yes")
                {
                    return 500;//abort with status code 
                }
                return null; //continue processing
            };


            pipelines.AfterRequest += ctx => {
                ctx.Response.Headers["X-Poc-RequestEndTime"] = DateTime.UtcNow.ToString();
            };

            
        }
    }
}
