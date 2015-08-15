using System;
using ASaed.POC.Nancy.VersionUtil.Data;

namespace ASaed.POC.Nancy.VersionUtil
{
    public interface IVersionProvider
    {
        AppVersion GetVersion();
    }

    public class DefaultVersionProvider : IVersionProvider
    {
        public AppVersion GetVersion()
        {
            return new AppVersion
            {
                SemanticVersion = "Not Implemented",
                CommitHash = "Not Implemented",
                CommitLink = "Not Implemented",
                BuildDateTime = DateTime.Now
            };
        }
    }
}