using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

using static nyoka_Client.Constants;

namespace nyoka_Client
{
    
    [Verb("available", HelpText = "List available resources from the repository server.")]
   
   public class AvailableOptions
    {
         public static ResourceType? resourceType;
        public static string prefix;
        public static int Available()
        {
            listAvailableResources(prefix: prefix, listType: resourceType);
            return 0;
        }
          public static void listAvailableResources(string prefix,ResourceType? listType)
        {
            try
            {
                List<ResourceType> resourcesToList = listType.HasValue ?
                    new List<ResourceType> { listType.Value } :
                    new List<ResourceType> { ResourceType.Code, ResourceType.Data, ResourceType.Model };

                PrintTable printTable = new PrintTable {
                    {Constants.HeaderStringType, 6},
                    {Constants.HeaderStringNameOfResource, 21},
                    {Constants.HeaderStringLatestVersion, 16},
                    {Constants.HeaderStringLocalVersion, 2},
                    {Constants.HeaderStringFileSize, 11},
                };

                foreach (ResourceType resourceType in resourcesToList)
                {
                    var availableResources = NetworkUtils.getAvailableResources(prefix,resourceType);

                    foreach (string resourceName in availableResources.resourceDescriptions.Keys.OrderBy(k => k))
                    {
                        string localVersionStr;
                        bool resourceExistsLocally = ResourceTypes.resourceFileExists(resourceType, resourceName);
                        if (resourceExistsLocally)
                        {
                            if (ResourceTypes.resourceVersionFileExists(resourceType, resourceName))
                            {
                                localVersionStr = ResourceTypes.getResourceVersion(resourceType, resourceName);
                            }
                            else
                            {
                                localVersionStr = "Unknown version";
                            }
                        }
                        else
                        {
                            localVersionStr = "Not present";
                        }

                        printTable.addRow(
                            PrintTable.doFormat(resourceType.ToString()),
                            PrintTable.doFormat(resourceName),
                            PrintTable.doFormat(availableResources.resourceDescriptions[resourceName].versionStr),
                            PrintTable.doFormat(localVersionStr),
                            PrintTable.doFormat(ResourceTypes.bytesToString(availableResources.resourceDescriptions[resourceName].byteCount))
                        );
                    }
                }

                Logger.logTable(printTable);
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