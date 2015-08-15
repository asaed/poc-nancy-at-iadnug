using Nancy;

namespace ASaed.POC.Nancy.VersionUtil
{
    public class VersionModule: NancyModule
    {
        public VersionModule(IVersionProvider versionProvider)
        {
            Get["/version"] = _ => versionProvider.GetVersion();
        }
    }
}
