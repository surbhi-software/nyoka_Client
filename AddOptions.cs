using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using static nyoka_Client.Constants;

namespace nyoka_Client
{
    [Verb("add", HelpText = "Download and add a resource file from repository server to local files/zementis server/zementis modeler.")]
    class AddOptions : IOptions
    {
        //[Option("alias", HelpText = "(Optional argument.)")]
        //string Alias { get; set; }
        public string ResourceName { get; set; }
        public static string prefix { get; set; }
        public static ResourceIdentifier resourceDescription { get; set; }

        public static int AddPackage(List<string> actionargs)
        {
            int intReturn = 0;
            if (CheckStatus(actionargs.ToList(), false))
            {
                Add_Package(prefix, resourceDescription);
                intReturn = 1;
            }

            return intReturn;
        }
        private static bool CheckStatus(List<string> actionArgs, bool successful)
        {
            string resourceStr;
            if (!(actionArgs.Count == 1 || actionArgs.Count == 2))
            {
                Logger.logError($"add action takes two arguments: alias(optional) ,resource name. Usage:");
                //logUsage();
                return successful = false;
                // return false;
            }
            else if (actionArgs.Count == 1)
            {
                prefix = null;
                resourceStr = actionArgs[0];
            }
            else
            {
                prefix = actionArgs[0];
                resourceStr = actionArgs[1];
            }
            try
            {
                resourceDescription = ResourceIdentifier.generateResourceIdentifier(resourceStr);
            }
            catch (ParseUtils.ArgumentProcessException ex)
            {
                Logger.logError($"Error: {ex.Message}");
                return successful = false;
            }

            return successful = true;
        }

        public static void Add_Package(string prefix, ResourceIdentifier resourceDescription)
        {
            try
            {
                ResourceType resourceType = resourceDescription.resourceType;
                string resourceName = resourceDescription.resourceName;

                // check if the resource is available from the server
                var availableResources = NetworkUtils.getAvailableResources(prefix, resourceType);
                if (!availableResources.resourceDescriptions.ContainsKey(resourceName))
                {
                    Logger.logError($"No resource called {resourceName} is available from the server.");
                    return;
                }

                string version = resourceDescription.version; // possible null
                var serverVersionInfo = NetworkUtils.getResourceVersions(prefix, resourceType, resourceName);

                if (version == null)
                {
                    version = serverVersionInfo.latestVersion;
                }
                else
                {
                    // check that the requested version is available from the server
                    if (!serverVersionInfo.versions.ContainsKey(version))
                    {
                        Logger.logError(
                            $"There is no version {version} available of resource {resourceName}. " +
                            $"These are the version(s) available: {string.Join(", ", serverVersionInfo.versions.Keys.ToList())}"
                        );
                        return;
                    }
                }

                // check if nyoka directories exists
                if (!ResourceTypes.hasNecessaryDirs())
                {
                    bool createDirs = Logger.askYesOrNo(
                        "Resource directories are not present in this directory. Create them now?"
                    );

                    if (createDirs)
                    {
                        InitOptions.tryCreateDirIfNonExistent();
                    }
                    else
                    {
                        Logger.logLine("Package add aborted");
                        return;
                    }
                }

                // check if the resource is already present
                if (ResourceTypes.resourceFileExists(resourceType, resourceName))
                {
                    bool continueAnyways = Logger.askYesOrNo(
                        $"{resourceType.ToString()} resource {resourceName} is already present. Delete and replace ?"
                    );

                    if (continueAnyways)
                    {
                        ResourceTypes.removeResourceFilesIfPresent(resourceType, resourceName);
                    }
                    else
                    {
                        Logger.logLine("Aborting resource add.");
                        return;
                    }
                }

                ResourceDependencyInfoContainer dependencies = NetworkUtils.getResourceDependencies(prefix, resourceType, resourceName, version);

                var depDescriptions = new Dictionary<ResourceType, Dictionary<string, ResourceDependencyInfoContainer.DependencyDescription>> {
                    { ResourceType.Code, dependencies.codeDeps },
                    { ResourceType.Data, dependencies.dataDeps },
                    { ResourceType.Model, dependencies.modelDeps },
                };

                bool downloadDependencies = false;

                // if there package has any dependencies
                if (depDescriptions.Any(kvPair => kvPair.Value.Count != 0))
                {
                    PrintTable table = new PrintTable {
                        {"Resource Type", 13},
                        {"Dependency Type", 15},
                        {"Name of Resource", 16},
                        {"Resource Version", 16},
                        {"File Size", 9},
                    };

                    foreach (var (depResourceType, deps) in depDescriptions.Select(x => (x.Key, x.Value)))
                    {
                        foreach (var (depName, depDescription) in deps.Select(x => (x.Key, x.Value)))
                        {
                            table.addRow(
                                depResourceType.ToString(),
                                depDescription.isDirectDependency ? "direct" : "indirect",
                                depName,
                                depDescription.versionStr,
                                ResourceTypes.bytesToString(depDescription.byteCount)
                            );
                        }
                    }

                    Logger.logLine($"Resource {resourceName} has these dependencies:");
                    Logger.logTable(table);
                    downloadDependencies = Logger.askYesOrNo("Download these dependencies?");

                    if (downloadDependencies) Logger.logLine("Downloading dependencies");
                    else Logger.logLine("Skipping downloading dependencies.");
                }

                if (downloadDependencies)
                {
                    var depsToDownload = new List<(ResourceType, string, string)>();
                    foreach (var (depResourceType, deps) in depDescriptions.Select(x => (x.Key, x.Value)))
                    {
                        foreach (var (depName, depDescription) in deps.Select(x => (x.Key, x.Value)))
                        {
                            bool continueWithDownload = true;

                            // Ask user whether to overwrite file if a file with this name exists locally already
                            if (ResourceTypes.resourceFileExists(depResourceType, depName))
                            {
                                if (ResourceTypes.resourceVersionFileExists(depResourceType, depName))
                                {
                                    string depLocalVersion = ResourceTypes.getResourceVersion(depResourceType, depName);

                                    if (depDescription.versionStr == depLocalVersion)
                                    {
                                        continueWithDownload = Logger.askYesOrNo(
                                            $"Dependency {depName} file exists locally at the required version " +
                                            $"({depDescription.versionStr}). Overwrite this file?"
                                        );
                                    }
                                    else
                                    {
                                        continueWithDownload = Logger.askYesOrNo(
                                            $"Dependency {depName} file exists locally at version {depLocalVersion}" +
                                            $" (depency required version is {depDescription.versionStr}). Overwrite this file?"
                                        );
                                    }
                                }
                                else
                                {
                                    continueWithDownload = Logger.askYesOrNo(
                                        $"Dependency {depName} file exists locally at an unknown version. Overwrite this file?"
                                    );
                                }
                            }

                            if (continueWithDownload)
                            {
                                depsToDownload.Add((depResourceType, depName, depDescription.versionStr));
                            }
                            else
                            {
                                Logger.logWarning($"Skipping download of dependency {depName}.");
                            }
                        }
                    }

                    foreach (var (depResourceType, depName, depVersion) in depsToDownload)
                    {
                        downloadPackage(prefix, depResourceType, depName, depVersion);
                    }
                }

                downloadPackage(prefix, resourceType, resourceName, version);
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
        private static void downloadPackage(string prefix, ResourceType resourceType, string resourceName, string version)
        {
            try
            {
                // check that resource is on server
                var availableResources = NetworkUtils.getAvailableResources(prefix, resourceType);
                if (!availableResources.resourceDescriptions.ContainsKey(resourceName))
                {
                    Logger.logError($"Could not find {resourceType.ToString()} resource with name {resourceName} on server");
                    return;
                }

                // check that resource on server has specified version
                var versionInfo = NetworkUtils.getResourceVersions(prefix, resourceType, resourceName);
                if (!versionInfo.versions.ContainsKey(version))
                {
                    Logger.logError(
                        $"Could not find version {version} on server. These are the version(s) available: {string.Join(", ", versionInfo.versions.Keys.ToList())}"
                    );
                    return;
                }

                using (FileStream resourceFileStream = FileSystem.createResourceFile(resourceType, resourceName))
                using (StreamWriter versionFileStream = ResourceIdentifier.createOrOverwriteResourceVersionFile(resourceType, resourceName))
                {
                    Task resourceFileTask = NetworkUtils.downloadResource(prefix, resourceType, resourceName, version, resourceFileStream);

                    Task resourceVersionTask = Task.Factory.StartNew(() =>
                    {
                        versionFileStream.WriteLine(version);
                    });

                    Task.WaitAll(resourceFileTask, resourceVersionTask);
                }
                Logger.logLine($"{resourceType} resource {resourceName} added");
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