using System;
using System.Xml;
namespace BingWallapapers.Helper
{
    public class DeepLinkHelper
    {
        private const string AppManifestName = "WMAppManifest.xml";
        private const string AppNodeName = "App";
        private const string AppProductIDAttributeName = "ProductID";

        public static string BuildApplicationDeepLink()
        {
            var applicationId = Guid.Parse(GetManifestAttributeValue(AppProductIDAttributeName));

            return BuildApplicationDeepLink(applicationId.ToString());
        }

        public static string BuildApplicationDeepLink(string applicationId)
        {
            return @"http://windowsphone.com/s?appid=" + applicationId;
        }

        public static string GetManifestAttributeValue(string attributeName)
        {
            var xmlReaderSettings = new XmlReaderSettings
            {
                XmlResolver = new XmlXapResolver()
            };

            using (var xmlReader = XmlReader.Create(AppManifestName, xmlReaderSettings))
            {
                xmlReader.ReadToDescendant(AppNodeName);

                if (!xmlReader.IsStartElement())
                {
                    throw new FormatException(AppManifestName + " is missing " + AppNodeName);
                }

                return xmlReader.GetAttribute(attributeName);
            }
        }
    }
}