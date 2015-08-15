using System;

namespace ASaed.POC.Nancy.VersionUtil.Data
{
    public class AppVersion
    {
        public string SemanticVersion { get; set; }
        public string CommitHash { get; set; }
        public string CommitLink { get; set; }
        public DateTime BuildDateTime { get; set; }
    }
}