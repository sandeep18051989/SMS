using System.Configuration;
using System;
using System.Xml;

namespace EF.Core
{
    public partial class CMSConfig : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new CMSConfig();

            var userAgentStringsNode = section.SelectSingleNode("UserAgentStrings");
            config.UserAgentStringsPath = GetString(userAgentStringsNode, "databasePath");
            return config;
        }

        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        public bool IgnoreStartupTasks { get; private set; }
        public string UserAgentStringsPath { get; private set; }
        public bool RedisCachingEnabled { get; private set; }
        public string RedisCachingConnectionString { get; private set; }
        public bool MultipleInstancesEnabled { get; private set; }
        public bool RunOnAzureWebsites { get; private set; }
        public string AzureBlobStorageConnectionString { get; private set; }
        public string AzureBlobStorageContainerName { get; private set; }
        public string AzureBlobStorageEndPoint { get; private set; }
        public bool DisableSampleDataDuringInstallation { get; private set; }
        public bool UseFastInstallationService { get; private set; }
        public string PluginsIgnoredDuringInstallation { get; private set; }
    }
}
