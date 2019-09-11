using System;
using System.Collections.Generic;
using System.IO;

using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;
using Microsoft.Extensions.Configuration;

namespace Bedrock.Shared.Tools.VersionBumper
{
    public class Configuration
    {
        #region Constructors
        public Configuration()
        {
            Initialize();
        }
        #endregion

        #region Public Properties
        public string ProjectsPath { get; set; }

        public string FileSearchPattern { get; set; }

        public string VersionPath { get; set; }

        public string DependencyVersionPath { get; set; }

        public string DependencyVersionAttribute { get; set; }

        public string DependencyIncludeAttribute { get; set; }

        public BumperType BumperType { get; set; }

        public IEnumerable<string> IncludeStartsWithPathSegments { get; set; }

        public IEnumerable<string> ExcludeEndsWithPathSegments { get; set; }

        public string SpecificVersion { get; set; }

        public bool IsPressAnyKeyToContinue { get; set; }

        public bool IsSaveChanges { get; set; }

        public bool IsQuiet { get; set; }
        #endregion

        #region Public Methods
        public List<string> Validate()
        {
            var returnValue = new List<string>();

            if (!Directory.Exists(ProjectsPath))
            {
                returnValue.Add($"\r\n\r\nPath {ProjectsPath} inaccessible or does not exist.");
            }

            return returnValue;
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", true, true)
                                    .Build();

            SetConfiguration(configuration);
        }

        private void SetConfiguration(IConfigurationRoot configuration)
        {
            ProjectsPath = configuration["projectsPath"];
            FileSearchPattern = configuration["fileSearchPattern"];

            VersionPath = configuration["versionPath"];

            DependencyVersionPath = configuration["dependencyVersionPath"];
            DependencyVersionAttribute = configuration["dependencyVersionAttribute"];
            DependencyIncludeAttribute = configuration["dependencyIncludeAttribute"];

            BumperType = (BumperType)Enum.Parse(typeof(BumperType), configuration["bumperType"], true);

            IncludeStartsWithPathSegments = configuration.GetSection("includeStartsWithPathSegments").Get<string[]>();
            ExcludeEndsWithPathSegments = configuration.GetSection("excludeEndsWithPathSegments").Get<string[]>();

            SpecificVersion = configuration["specificVersion"];
            IsPressAnyKeyToContinue = bool.Parse(configuration["isPressAnyKeyToContinue"]);

            IsSaveChanges = bool.Parse(configuration["isSaveChanges"]);
            IsQuiet = bool.Parse(configuration["isQuiet"]);
        }
        #endregion
    }
}
