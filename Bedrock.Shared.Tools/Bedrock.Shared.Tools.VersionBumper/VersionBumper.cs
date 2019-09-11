using System;
using System.Collections;
using System.Text;

using Bedrock.Shared.Tools.VersionBumper.Bumper;
using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;

using BumperLogger = Bedrock.Shared.Tools.VersionBumper.Logger;

namespace Bedrock.Shared.Tools.VersionBumper
{
    public class VersionBumper
    {
        #region Constructors
        public VersionBumper()
        {
            Initialize();
        }
        #endregion

        #region Protected Properties
        protected Configuration Configuration { get; set; }

        protected BumperLogger.ILogger Logger { get; set; }

        protected BumperProvider BumperProvider { get; set; }

        protected FileUpdater FileUpdater { get; set; }
        #endregion

        #region Public Methods
        public void Bump(string[] arguments)
        {
            var argumentList = new Hashtable(5);
            var requiredArguments = new string[] { };
            var errorFlag = false;

            if (arguments.Length > 0 && arguments[0].IndexOf("?") > 0)
            {
                WriteHelpMessage();
            }
            else
            {
                HarvestArguments(arguments, argumentList, ref errorFlag);
                EnsureArgumentsComplete(requiredArguments, argumentList, ref errorFlag);

                var canRun = !errorFlag && EnsureConfigurationValidation();

                if (canRun)
                    Run();
            }

            HandlePressAnyKey();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            Configuration = new Configuration();
            Logger = new BumperLogger.Logger(Configuration);

            BumperProvider = new BumperProvider(Configuration, Logger);
            FileUpdater = new FileUpdater(BumperProvider, Configuration, Logger);
        }

        private void WriteHelpMessage()
        {
            Logger.Log("\r\n-----------------------------------------------------------------------------------------------\r\n");

            Logger.Log("Usage:\r\n\t\t dotnet Bedrock.Shared.Tools.VersionBumper.dll");
            Logger.Log("\t\t\t/p:<solution path>");
            Logger.Log("\t\t\t/t:<bumper type>");
            Logger.Log("\t\t\t/k:<press key to continue>");
            Logger.Log("\t\t\t/v:<specific version>");
            Logger.Log("\t\t\t/s:<save changes>");
            Logger.Log("\t\t\t/q:<quiet>");

            Logger.Log("\r\nExample:\r\n\t\t dotnet Bedrock.Shared.Tools.VersionBumper.dll");
            Logger.Log("\t\t\t/p:\"C:\\Bedrock\\Bedrock.Shared\"");
            Logger.Log("\t\t\t/t:Patch");
            Logger.Log("\t\t\t/k:True");
            Logger.Log("\t\t\t/v:1.0.1.15");
            Logger.Log("\t\t\t/s:True");
            Logger.Log("\t\t\t/q:False");

            Logger.Log("\r\nDefaults:");
            Logger.Log($"\t\t\t/p:{Configuration.ProjectsPath}");
            Logger.Log($"\t\t\t/t:{Configuration.BumperType}");
            Logger.Log($"\t\t\t/k:{Configuration.IsPressAnyKeyToContinue}");
            Logger.Log($"\t\t\t/v:{Configuration.SpecificVersion}");
            Logger.Log($"\t\t\t/s:{Configuration.IsSaveChanges}");
            Logger.Log($"\t\t\t/q:{Configuration.IsQuiet}");

            Logger.Log("\r\n-----------------------------------------------------------------------------------------------\r\n");
            Logger.Log("/?                           -  Print this message");
            Logger.Log("/p:<solution path>           -  The solution path containing projects to bump");
            Logger.Log("/t:<major|minor|patch|whole> -  Part of version to bump");
            Logger.Log("/k:<true|false>              -  Press any key to continue?");
            Logger.Log("/v:<specific version>        -  Bump to this specific version");
            Logger.Log("/s:<true|false>              -  Set true to save file changes");
            Logger.Log("/q:<true|false>              -  Set true to silence output");

            Logger.Log("\r\n-----------------------------------------------------------------------------------------------\r\n");
            Logger.Log("*   VersionBumper will increment the Major, Minor or Patch component for all projects and project dependencies.");
            Logger.Log("*   If you would prefer to bump the version to a specific whole version, use the /v flag.");
            Logger.Log("*   If you specify both the /t and /v flag, the last one wins.");
            Logger.Log("*   If you specify /t:whole, but do not specify the /v flag, the version will be the default configuration value.");
            Logger.Log("*   Because this utility can potentially change many files, you must explicitly opt-in for file saves.");
            Logger.Log("\r\n-----------------------------------------------------------------------------------------------\r\n");
        }

        private void HarvestArguments(string[] arguments, Hashtable argumentList, ref bool errorFlag)
        {
            var tempCommand = default(string);
            var tempCommandValue = default(string);

            foreach (string commandSwitch in arguments)
            {
                // Ensure params start with /x:
                if ((commandSwitch.Length >= 3) && (commandSwitch.IndexOf("/", 0, 1) != -1) && (commandSwitch.IndexOf(":", 2, 1) != -1))
                {
                    tempCommand = commandSwitch.Substring(1, commandSwitch.IndexOf(":") - 1);
                    tempCommandValue = commandSwitch.Substring(commandSwitch.IndexOf(":") + 1);
                }
                else
                {
                    Logger.Log("\r\n\r\nParameter not in correct format.");
                    errorFlag = true;
                }

                // Grab params and make sure all accounted for
                // Make sure param has value
                if ((tempCommandValue != null && tempCommandValue.Length > 0))
                {
                    switch (tempCommand)
                    {
                        case "p":
                            {
                                Configuration.ProjectsPath = tempCommandValue;
                                argumentList[tempCommand] = tempCommandValue;
                                break;
                            }
                        case "t":
                            {
                                var bumperType = default(BumperType);

                                if(Enum.TryParse(tempCommandValue, true, out bumperType))
                                {
                                    Configuration.BumperType = bumperType;
                                    argumentList[tempCommand] = tempCommandValue;
                                }
                                else
                                {
                                    Logger.Log($"\r\n\rBumper Type {tempCommandValue} is invalid.");
                                    errorFlag = true;
                                }

                                break;
                            }
                        case "k":
                            {
                                Configuration.IsPressAnyKeyToContinue = bool.Parse(tempCommandValue);
                                argumentList[tempCommand] = tempCommandValue;
                                break;
                            }
                        case "v":
                            {
                                Configuration.BumperType = BumperType.Whole;
                                Configuration.SpecificVersion = tempCommandValue;

                                argumentList[tempCommand] = tempCommandValue;
                                break;
                            }
                        case "s":
                            {
                                Configuration.IsSaveChanges = bool.Parse(tempCommandValue);
                                argumentList[tempCommand] = tempCommandValue;
                                break;
                            }
                        case "q":
                            {
                                Configuration.IsQuiet = bool.Parse(tempCommandValue);
                                argumentList[tempCommand] = tempCommandValue;
                                break;
                            }
                    }
                }
                else
                {
                    Logger.Log($"\r\n\r\nCommand {tempCommand} must have a value.  Please check and try again.");
                    errorFlag = true;
                }

                if (errorFlag)
                {
                    argumentList.Clear();
                    break;
                }
            }
        }

        private void EnsureArgumentsComplete(string[] requiredArguments, Hashtable argumentList, ref bool errorFlag)
        {
            var error = new StringBuilder();
            var isFound = false;

            foreach (string argumentName in requiredArguments)
            {
                foreach (var tempArgumentName in argumentList.Keys)
                {
                    if (argumentName == (string)tempArgumentName)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    errorFlag = true;
                    error.Append("\"" + argumentName + "\" parameter required\n");
                }

                isFound = false;
            }

            Logger.Log("\n" + error.ToString());
        }

        private bool EnsureConfigurationValidation()
        {
            var configurationErrors = Configuration.Validate();

            foreach (var error in configurationErrors)
                Logger.Log(error);

            return configurationErrors.Count == 0;
        }

        private void Run()
        {
            Logger.Log("Begin Update...\r\n");
            FileUpdater.WalkDirectory(Configuration.ProjectsPath);
            Logger.Log("Update Complete.");
        }

        private void HandlePressAnyKey()
        {
            if (Configuration.IsPressAnyKeyToContinue)
            {
                Logger.Log("Press any key to continue...");
                Console.ReadKey();
            }
        }
        #endregion
    }
}
