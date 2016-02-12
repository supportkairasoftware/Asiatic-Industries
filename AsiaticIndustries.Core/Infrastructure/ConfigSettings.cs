using System;
using System.Configuration;

namespace AsiaticIndustries.Core.Infrastructure
{
    public class ConfigSettings
    {
        public static readonly string SiteBaseUrl = ConfigurationManager.AppSettings["SiteBaseUrl"];


        public static readonly string BasePath = ConfigurationManager.AppSettings["BasePath"];
        public static readonly bool EnableBundlingMinification = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableBundlingMinification"]);
        public static readonly string PageSize = ConfigurationManager.AppSettings["PageSize"];
        public static readonly int RememberMeDuration = Convert.ToInt32(ConfigurationManager.AppSettings["RememberMeDuration"]);
        public static string TempPath = ConfigurationManager.AppSettings["TempPath"];
    }
}
