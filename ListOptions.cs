using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    [Verb("list", HelpText = "List resources in local files.")]
   public class ListOptions
    {
        [Option("type", HelpText = "Get resource for this type.")]
        public string resource { get; set; }
        public Constants.ResourceType? resourceType;
               //public ListOptions(List<string> actionArgs)
         // {
        //       if (actionArgs.Count > 1)
        //      {
        //         Logger.logError($"list action takes one optional argument: resource type. Usage:");
        //         lstResources(resourceType);
        //          //successful = false;
        //          return;
        //      }
        //      if (actionArgs.Count == 1)
        //      {
        //          string resourceTypeStr = actionArgs[0];
        //           try
        //          {
        //              resourceType = ParseUtils.parseResourceType(resourceTypeStr);
        //          }
        //          catch (ParseUtils.ArgumentProcessException ex)
        //          {
        //              Logger.logError($"Error: {ex.Message}");
        //             // return 0;
        //          }
        //      }
        //      //return 1;
         // }

        public static int ListResources(Constants.ResourceType? listType, List<string> actionArgs)
        {
            if (actionArgs.Count > 1)
            {
                string resourceTypeStr = actionArgs[0];
                Logger.logError($"list action takes one optional argument: resource type. Usage:");
                listType = ParseUtils.parseResourceType(resourceTypeStr);
                lstResources(listType);
            }
            if (actionArgs.Count == 1)
            {
                try
                {
                    lstResources(listType);
                }
                catch (ParseUtils.ArgumentProcessException ex)
                {
                    Logger.logError($"Error: {ex.Message}");
                    return 0;
                }
            }
            return 1;
        }
        public static void lstResources(Constants.ResourceType? listType)
        {
            try
            {
                if (!ResourceTypes.hasNecessaryDirs())
                {
                    Logger.logError($"Missing some or all resource directories in current directory. Try running {Constants.APPLICATION_ALIAS} init?");
                    return;
                }
                PrintTable table = new PrintTable {
                    {Constants.HeaderStringType, 6},
                    {Constants.HeaderStringNameOfResource, 21},
                    {Constants.HeaderStringVersion, 16},
                    {Constants.HeaderStringFileSize, 11},
                };

                List<Constants.ResourceType> resourcesToList = listType.HasValue ?
                   new List<Constants.ResourceType> { listType.Value } :
                   new List<Constants.ResourceType> { Constants.ResourceType.Code,
                     Constants.ResourceType.Data, Constants.ResourceType.Model };

                foreach (Constants.ResourceType resourceType in resourcesToList)
                {
                    foreach (string resourceName in ResourceTypes.resourceNames(resourceType))
                    {
                        string version;
                        if (ResourceTypes.resourceVersionFileExists(resourceType, resourceName))
                        {
                            version = ResourceTypes.getResourceVersion(resourceType, resourceName);
                        }
                        else
                        {
                            version = "Unknown version";
                        }

                        long fileSize = ResourceTypes.getResourceSize(resourceType, resourceName);

                        table.addRow(
                            PrintTable.doFormat(resourceType.ToString()),
                            PrintTable.doFormat(resourceName),
                            PrintTable.doFormat(version),
                            PrintTable.doFormat(ResourceTypes.bytesToString(fileSize))
                        );
                    }
                }
                Logger.logTable(table);
            }
            catch
            { }
        }
    }
}