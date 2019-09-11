using System;
using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;

namespace Bedrock.Shared.Tools.VersionBumper.Bumper
{
    public class BumperMajor : BumperBase
    {
        #region Constructors
        public BumperMajor(Configuration configuration) : base(BumperType.Major, configuration) { }
        #endregion

        #region IBumper Methods
        public override string Bump(string currentVersion)
        {
            var returnValue = currentVersion;
            var versionParts = currentVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var versionPartNumbers = Array.ConvertAll(versionParts, int.Parse);

            switch (versionParts.Length)
            {
                case 4:
                case 3:
                    {
                        returnValue = $"{++versionPartNumbers[0]}.{versionPartNumbers[1]}.{versionPartNumbers[2]}";
                        break;
                    }
                case 2:
                    {
                        returnValue = $"{++versionPartNumbers[0]}.{versionPartNumbers[1]}.{0}";
                        break;
                    }
                case 1:
                    {
                        returnValue = $"{++versionPartNumbers[0]}.{0}.{0}";
                        break;
                    }
            }

            return returnValue;
        }
        #endregion
    }
}
