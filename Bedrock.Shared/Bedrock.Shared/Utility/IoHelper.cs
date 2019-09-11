using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bedrock.Shared.Utility
{
    public static class IoHelper
    {
        #region Public Methods
        public static string GetExecutionPathLocation(params object[] args)
        {
            var stringArgs = new List<object>(args);
            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stringArgs.Insert(0, executionPath);

            return string.Concat(stringArgs);
        }

        public static void EnsurePath(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            finally { }
        }

        public static void CleanPath(string path, bool isCleanWritePath)
        {
            try
            {
                if (isCleanWritePath)
                {
                    var pathInfo = new DirectoryInfo(path);

                    foreach (var file in pathInfo.GetFiles())
                        file.Delete();
                }
            }
            finally { }
        }

        public static void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
        #endregion
    }
}
