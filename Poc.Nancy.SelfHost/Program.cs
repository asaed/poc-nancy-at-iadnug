using System;
using Nancy.Hosting.Self;

namespace ASaed.POC.Nancy.SelfHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new HostConfiguration(){
                UrlReservations = new UrlReservations(){
                    CreateAutomatically = true
                }
            };
            const string httpLocalhost = "http://localhost:9005";
            using (var host = new NancyHost(config, new Uri(httpLocalhost)))
            {
                Console.WriteLine("Starting up web app .... ");
                host.Start();
                Console.WriteLine("App running on " + httpLocalhost);

                Console.WriteLine("Press any key to terminate application .... ");
                Console.ReadLine();

                host.Stop();
            }
        }
    }
}
