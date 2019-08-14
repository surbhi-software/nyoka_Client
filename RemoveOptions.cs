using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using static nyoka_Client.Constants;

namespace nyoka_Client
{
    [Verb("remove",
    HelpText = "Removes a code resource named programName.py from the code folder in the current directory.")]
    class RemoveOptions : IOptions
    {
        public string Alias { get; set; }
        public string ResourceName { get; set; }
        public static ResourceIdentifier resourceIdentifier;

        public static void remove(ResourceIdentifier resourceDescription)
        {
            try
            {
                string resourceName = resourceDescription.resourceName;
               // Console.WriteLine(resourceName);
                ResourceType resourceType = resourceDescription.resourceType;
               // Console.WriteLine(resourceType);
                InitOptions.initDirectories();

                Logger.logLine($"Removing {resourceType.ToString().ToLower()} resource \"{resourceName}\"");
                if (!ResourceTypes.resourceFileExists(resourceType, resourceName))
                {
                    Logger.logError($"{resourceType} resource \"{resourceName}\" does not exist");
                    return;
                }

                if (resourceDescription.version != null)
                {
                    if (ResourceTypes.resourceVersionFileExists(resourceType, resourceName))
                    {
                        string localVersion = ResourceTypes.getResourceVersion(resourceType, resourceName);
                        if (localVersion != resourceDescription.version)
                        {
                            bool removeAnyways = Logger.askYesOrNo($"Present version is {localVersion}, not {resourceDescription.version}. Remove anyways?");
                            if (!removeAnyways)
                            {
                                Logger.logLine("Aborting resource removal.");
                                return;
                            }
                        }
                    }
                    else
                    {
                        bool removeAnyways = Logger.askYesOrNo($"Local file {resourceName} has unknown version. Remove anyways?");
                        if (!removeAnyways)
                        {
                            Logger.logLine("Aborting resource removal.");
                            return;
                        }
                    }
                }

                ResourceTypes.removeResourceFilesIfPresent(resourceType, resourceName);
                Logger.logLine("Resource removed");
            }
            catch (Exception ex)
            {
                Logger.logError($"File System Error: " + ex.Message);
            }
        }

        public static int RemovePackage(List<string> actionArgs)
        {
           // Console.WriteLine(actionArgs[0]+ " " + actionArgs[1]);
            

            string resourceStr = actionArgs[1];
            try
            {
                resourceIdentifier = ResourceIdentifier.generateResourceIdentifier(resourceStr);
            }
            catch (ParseUtils.ArgumentProcessException ex)
            {
                Logger.logError($"Error: {ex.Message}");
                //successful = false;
                //return;
            }

            //successful = true;
            

             remove(resourceIdentifier);
            return 1;
        }
    }
}