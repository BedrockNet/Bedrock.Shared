using System.IO;
using System.Linq;
using System.Xml;

using Bedrock.Shared.Tools.VersionBumper.Bumper;
using Bedrock.Shared.Tools.VersionBumper.Bumper.Interface;

using Bedrock.Shared.Tools.VersionBumper.Logger;

namespace Bedrock.Shared.Tools.VersionBumper
{
    public class FileUpdater
    {
        #region Constructors
        public FileUpdater(BumperProvider bumperProvider, Configuration configuration, ILogger logger)
        {
            BumperProvider = bumperProvider;
            Configuration = configuration;
            Logger = logger;
        }
        #endregion

        #region Protected Properties
        protected IBumperProvider BumperProvider { get; set; }

        protected Configuration Configuration { get; set; }

        protected ILogger Logger { get; set; }
        #endregion

        #region Public Methods
        public void WalkDirectory(string path, bool isRoot = true)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directories = directoryInfo
                                .GetDirectories()
                                .Where(d => Configuration.IncludeStartsWithPathSegments.Any(ips => d.Name.StartsWith(ips)));

            if (!isRoot)
                directories = directories.Where(d => !Configuration.ExcludeEndsWithPathSegments.Any(eps => d.Name.EndsWith(eps)));

            foreach (var directory in directories)
            {
                var files = directory.GetFiles(Configuration.FileSearchPattern);

                foreach (var file in files)
                {
                    UpdateFile(file);
                }

                WalkDirectory(directory.FullName, false);
            }
        }
        #endregion

        #region Private Methods
        private void UpdateFile(FileInfo file)
        {
            Logger.Log($"Project File: {file.FullName}");

            var filePath = file.FullName;
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(filePath);

            UpdateVersion(xmlDocument);
            UpdateDependencyVersions(xmlDocument);
            RemoveOutputPath(xmlDocument);

            if (Configuration.IsSaveChanges)
                xmlDocument.Save(filePath);

            Logger.Log("\r");
        }

        private void UpdateVersion(XmlDocument xmlDocument)
        {
            var node = xmlDocument.SelectSingleNode(Configuration.VersionPath);
            node.InnerText = BumperProvider.Bump(node.InnerText, Configuration.BumperType);

            Logger.Log($"Updated to {node.InnerText}");
        }

        private void UpdateDependencyVersions(XmlDocument xmlDocument)
        {
            var nodes = xmlDocument.SelectNodes(Configuration.DependencyVersionPath);

            foreach (XmlNode node in nodes)
            {
                var includeAttribute = node.Attributes[Configuration.DependencyIncludeAttribute];
                var versionAttribute = node.Attributes[Configuration.DependencyVersionAttribute];

                versionAttribute.Value = BumperProvider.Bump(versionAttribute.Value, Configuration.BumperType);

                Logger.Log($"Dependency {includeAttribute.InnerText} updated to {versionAttribute.Value}");
            }
        }

        private void RemoveOutputPath(XmlDocument xmlDocument)
        {
            var node = xmlDocument.SelectSingleNode("//PropertyGroup/OutputPath");

            if(node != null)
                node.ParentNode.RemoveChild(node);
        }
        #endregion
    }
}
