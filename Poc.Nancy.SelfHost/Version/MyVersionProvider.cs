using System;
using System.Diagnostics;
using System.Reflection;
using ASaed.POC.Nancy.VersionUtil;
using ASaed.POC.Nancy.VersionUtil.Data;

namespace ASaed.POC.Nancy.SelfHost.Version
{
    
    public class MyVersionProvider : IVersionProvider
    {
        public AppVersion GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return new AppVersion
            {
                SemanticVersion = fvi.FileVersion,
                BuildDateTime = DateTime.Now
            };
        }
    }
    
}