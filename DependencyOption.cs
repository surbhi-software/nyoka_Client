using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

using static nyoka_Client.Constants;

namespace nyoka_Client
{[Verb("dependencies", HelpText = "List dependencies of resource.")]
   
    public class DependencyOption
    {
        public static ResourceIdentifier resourceIdentifier;
        public static string prefix;
         public DependencyOption(List<string> actionArgs, out bool successful)
        {
            if (actionArgs.Count != 1)
            {
                Logger.logError($"dependencies action takes one argument: resource name. Usage:");
                //logUsage();
                successful = false;
                return;
            }

            string resourceStr = actionArgs[0];
            try
            {
                resourceIdentifier = ResourceIdentifier.generateResourceIdentifier(resourceStr);
            }
            catch (ParseUtils.ArgumentProcessException ex)
            {
                Logger.logError($"Error: {ex.Message}");
                successful = false;
                return;
            }

            successful = true;
        }


        public static int ListAllDependencies()
        {
            listDependencies(prefix: prefix, resourceIdentifier);
            return 0;
        }
        public static void listDependencies(string prefix, ResourceIdentifier resourceDescription)
        {
            string resourceName = resourceDescription.resourceName;
            ResourceType resourceType = resourceDescription.resourceType;
            string version = resourceDescription.version;

            try
            {
                // check if this resource exists on server
                var availableResources = NetworkUtils.getAvailableResources(prefix, resourceType);
                if (!availableResources.resourceDescriptions.ContainsKey(resourceName))
                {
                    Logger.logError($"{resourceType.ToString()} resource {resourceName} could not be found on server");
                    return;
                }
                if (version == null)
                {
                    if (
                        ResourceTypes.resourceFileExists(resourceType, resourceName)
                        && ResourceTypes.resourceVersionFileExists(resourceType, resourceName)
                    )
                    {
                        version = ResourceTypes.getResourceVersion(resourceType, resourceName);
                    }
                    else
                    {
                        var versionInfo = NetworkUtils.getResourceVersions(prefix, resourceType, resourceName);
                        version = versionInfo.latestVersion;
                    }
                }
                // check if user-specified version exists on the server at the given version
                else
                {
                    var versionInfo = NetworkUtils.getResourceVersions(prefix, resourceType, resourceName);
                    if (!versionInfo.versions.ContainsKey(version))
                    {
                        Logger.logError("Server does not report having a version \"{version}\" available for {resourceName}");
                    }
                }

                Logger.logLine($"Showing dependencies of {resourceName}, version {version}");

                ResourceDependencyInfoContainer deps = NetworkUtils.getResourceDependencies(prefix, resourceType, resourceName, version);

                PrintTable table = new PrintTable {
                    {"Resource Type", 13},
                    {"Dependency Type", 15},
                    {"Name of Resource", 15},
                    {"Resource Version", 15},
                    {"File Size", 10},
                };

                var availableResourcesInfo = new Dictionary<ResourceType, AvailableResourcesInfoContainer> {
                    { ResourceType.Code, NetworkUtils.getAvailableResources(prefix, ResourceType.Code) },
                    { ResourceType.Data, NetworkUtils.getAvailableResources(prefix, ResourceType.Data) },
                    { ResourceType.Model, NetworkUtils.getAvailableResources(prefix, ResourceType.Model) },
                };

                var showDepDict = new Dictionary<ResourceType, Dictionary<string, ResourceDependencyInfoContainer.DependencyDescription>>() {
                    { ResourceType.Code, deps.codeDeps },
                    { ResourceType.Data, deps.dataDeps },
                    { ResourceType.Model, deps.modelDeps },
                };

                foreach (var (dependenciesType, descriptions) in showDepDict.Select(x => (x.Key, x.Value)))
                {
                    foreach (var (dependencyName, dependencyDescription) in descriptions.Select(x => (x.Key, x.Value)))
                    {
                        table.addRow(
                            dependenciesType.ToString(),
                            dependencyDescription.isDirectDependency ? "direct" : "indirect",
                            dependencyName,
                            dependencyDescription.versionStr,
                            ResourceTypes.bytesToString(availableResourcesInfo[dependenciesType].resourceDescriptions[dependencyName].byteCount)
                        );
                    }
                }
                Logger.logTable(table);
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
    }
}