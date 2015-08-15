using ASaed.POC.Nancy.VersionUtil;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NLog;

namespace ASaed.POC.Nancy.SelfHost.Mvc
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            logger.Info("Using custom bootstapper");
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
//            Uncomment the following line to make IOC resolution deterministic
//            existingContainer.Register<IVersionProvider, MyVersionProvider>().AsSingleton();
        }
    }
}