
using System;
using System.Collections.Generic;
using System.IO;
using static nyoka_Client.Constants;
using System.Linq;
namespace nyoka_Client
{
    public class ResourceTypes
    {
        public static bool hasNecessaryDirs()
        {
            try
            {
                foreach (string dirName in Constants.dirNames)
                {
                    if (!Directory.Exists(dirName))
                    {
                        return false;
                    }
                    if (!Directory.Exists(Path.Join(dirName, Constants.nyokaFolderName)))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (System.Exception)
            {
                throw new Exception("Failed to check for necessary directories and files");
            }
        }

        public static IEnumerable<string> resourceNames(ResourceType resourceType)
        {
            try
            {
                return new DirectoryInfo(Constants.resourceDirPath(resourceType))
                .EnumerateFiles().Select(file => file.Name);
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to get list of {resourceType.ToString()} resources from file system");
            }
        }

        internal static bool resourceVersionFileExists(ResourceType resourceType, string resourceName)
        {
            try
            {
                return File.Exists(Path.Join(resourceDirPath(resourceType), nyokaFolderName, resourceName + nyokaVersionExtension));
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to check for existence of version file for {resourceType.ToString().ToLower().ToLower()} resource {resourceName}");
            }
        }

        internal static string getResourceVersion(ResourceType resourceType, string resourceName)
        {
            try
            {
                return File.ReadAllText(Path.Join(resourceDirPath(resourceType), nyokaFolderName,
                resourceName + Constants.nyokaVersionExtension)).Trim();
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to read metadata file for {resourceType.ToString().ToLower()} resource {resourceName}");
            }
        }

        public static bool resourceFileExists(ResourceType resourceType, string resourceName)
        {
            try
            {
                return File.Exists(Path.Join(resourceDirPath(resourceType), resourceName));
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to check file system for whether {resourceType.ToString().ToLower()} resource {resourceName} exists");
            }
        }
        public static string bytesToString(long bytes)
        {
            const long KSize = 1024;
            const long MSize = 1048576;
            const long GSize = 1073741824;
            const long TSize = 1099511627776;

            long unit;
            string suffix;
            if (bytes < KSize)
            {
                unit = 1;
                suffix = "B";
            }
            else if (bytes < MSize)
            {
                unit = KSize;
                suffix = "KB";
            }
            else if (bytes < GSize)
            {
                unit = MSize;
                suffix = "MB";
            }
            else if (bytes < TSize)
            {
                unit = GSize;
                suffix = "GB";
            }
            else
            {
                unit = TSize;
                suffix = "TB";
            }

            float dividedByUnits = bytes / ((float)unit);

            // represent either as integer or to two decimal places
            string numToString = dividedByUnits % 1 == 0 ? dividedByUnits.ToString() : string.Format("{0:0.00}", dividedByUnits);

            return $"{numToString} {suffix}";
        }

        public static long getResourceSize(ResourceType resourceType, string resourceName)
        {
            try
            {
                return new FileInfo(Path.Join(resourceDirPath(resourceType), resourceName)).Length;
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to determine file size of file for {resourceType.ToString().ToLower()} resource {resourceName}");
            }
        }

        public static StreamWriter createOrOverwriteResourceVersionFile(ResourceType resourceType, string resourceName)
        {
            try
            {
                string filePath = Path.Join(resourceDirPath(resourceType), nyokaFolderName, resourceName + nyokaVersionExtension);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return File.CreateText(filePath);
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to create metadata file for {resourceType.ToString().ToLower()} resource {resourceName}");
            }
        }
        public static FileStream readResourceFile(ResourceType resourceType, string fileName)
        {
            try
            {
                string filePath = Path.Join(resourceDirPath(resourceType), fileName);
                return File.OpenRead(filePath);
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to open {resourceType.ToString().ToLower()} resource {fileName}. Does this file exist?");
            }
        }



    }
}