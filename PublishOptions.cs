using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

using static nyoka_Client.Constants;

namespace nyoka_Client
{
    [Verb("publish", HelpText = "Publish a resource in local files to the server.")]

    public class PublishOptions
    {
        public string prefix { get; set; }
        public ResourceIdentifier resourceDescription { get; set; }
        public IEnumerable<ResourceIdentifier> deps { get; set; }
        public static void publishResource(string prefix, ResourceIdentifier resourceDescription, IEnumerable<ResourceIdentifier> deps)
        {
            try
            {
                PublishDepsInfoContainer publishDepsInfo = new PublishDepsInfoContainer();

                foreach (ResourceIdentifier depDescription in deps)
                {
                    if (depDescription.version == null)
                    {

                        Logger.logError(
                            "The versions of dependencies must be supplied. For example, " +
                            "\"dependency.csv\" does not include version, \"dependency.csv@1.2.3\" does."
                        );
                        return;
                    }

                    var publishDepDescription = new PublishDepsInfoContainer.PublishDepDescription(depDescription.version);
                    if (depDescription.resourceType == ResourceType.Code)
                    {
                        publishDepsInfo.codeDeps[depDescription.resourceName] = publishDepDescription;
                    }
                    else if (depDescription.resourceType == ResourceType.Data)
                    {
                        publishDepsInfo.dataDeps[depDescription.resourceName] = publishDepDescription;
                    }
                    else if (depDescription.resourceType == ResourceType.Model)
                    {
                        publishDepsInfo.modelDeps[depDescription.resourceName] = publishDepDescription;
                    }
                }

                string resourceName = resourceDescription.resourceName;
                ResourceType resourceType = resourceDescription.resourceType;
                string publishVersion = resourceDescription.version;

                // check that user has provided version to publish file as
                if (publishVersion == null)
                {
                    publishVersion = "1.0";
                    Logger.logLine($"Using default version {publishVersion}");
                }

                if (!ResourceTypes.hasNecessaryDirs())
                {
                    Logger.logError($"Could not find nyoka resource folders in current directory. Try running {Constants.APPLICATION_ALIAS} init?");
                    return;
                }

                // If a file to publish with the given name can't be found
                if (!ResourceTypes.resourceFileExists(resourceType, resourceName))
                {
                    Logger.logError($"Resource with name {resourceName} not found.");
                    return;
                }

                var resourcesOnServer = NetworkUtils.getAvailableResources(prefix, resourceType);

                // If this resource already exists on server
                if (resourcesOnServer.resourceDescriptions.ContainsKey(resourceName))
                {
                    ResourceVersionsInfoContainer serverVersionsInfo = NetworkUtils.getResourceVersions(prefix, resourceType, resourceName);

                    // If this resource exists with the same version on server
                    if (serverVersionsInfo.versions.ContainsKey(publishVersion))
                    {
                        bool continueAnyways = Logger.askYesOrNo(
                            $"Version {publishVersion} of {resourceType.ToString()} resource " +
                            $"{resourceName} already exists on server. Overwrite?"
                        );

                        if (!continueAnyways)
                        {
                            Logger.logLine("Aborting publish.");
                            return;
                        }
                        else
                        {
                            Logger.logLine("Overwriting resource on server.");
                        }
                    }
                }

                Logger.logLine("Opening file.");
                FileStream fileStream = ResourceTypes.readResourceFile(resourceType, resourceName);

                Logger.logLine("Uploading file.");
                NetworkUtils.publishResource(
                    prefix,
                    fileStream,
                    resourceType,
                    resourceName,
                    publishVersion,
                    publishDepsInfo
                );

                // create or overwrite version file locally for this resource to be the publishVersion
                using (var versionFileStream = ResourceTypes.createOrOverwriteResourceVersionFile(resourceType, resourceName))
                {
                    versionFileStream.WriteLine(publishVersion);
                }
            }
            catch (NetworkUtils.NetworkUtilsException ex)
            {
                Logger.logError($"Network Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.logError($"File System Error: " + ex.Message);
            }

        }
        public static int Publish(string prefix, ResourceIdentifier resourceDescription, IEnumerable<ResourceIdentifier> deps)
        {
            Console.WriteLine(prefix);
            publishResource(prefix, resourceDescription, deps);
            return 1;
        }
    }
}