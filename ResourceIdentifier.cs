using nyoka_Client;
using static nyoka_Client.Constants;
using static nyoka_Client.ParseUtils;

namespace nyoka_Client
{
    public class ResourceIdentifier
    {
        public string resourceName;
        public string version;
        public Constants.ResourceType resourceType;

        public ResourceIdentifier(string resourceName, Constants.ResourceType resourceType)
        {
            this.resourceName = resourceName;
            this.resourceType = resourceType;
            this.version = null;
        }
        public ResourceIdentifier(string resourceName, Constants.ResourceType resourceType, string version)
        {
            this.resourceName = resourceName;
            this.resourceType = resourceType;
            this.version = version;
        }
         public static ResourceIdentifier generateResourceIdentifier(string resourceStr)
        {
            string[] splitByAt = resourceStr.Split('@');

            string version;
            string resourceNameStr;

            // If there is no @ symbol in the string to separate name from version
            if (splitByAt.Length == 1)
            {
                version = null; // redundant?
                resourceNameStr = splitByAt[0];
            }
            // If there is one @ symbol in the string to separate name from version
            else if (splitByAt.Length == 2)
            {
                version = splitByAt[1];
                resourceNameStr = splitByAt[0];
            }
            // If there is more than one @ symbol in the string
            else
            {
                throw new ArgumentProcessException(
                    $"Could not process \"{resourceStr}\": Only one @ symbol is permitted in a resource name"
                );
            }

            string resourceName = removeFolderPrefixIfPresent(resourceNameStr);

            ResourceType resourceType = inferResourceTypeFromResourceName(resourceName);

            if (version != null)
            {
                // validate version string
                validateVersionString(resourceStr, version);
            }
            
            validateFileName(resourceName);

            return new ResourceIdentifier(resourceName, resourceType, version);
        }
       private static string removeFolderPrefixIfPresent(string resourceNameStr)
        {
            if (resourceNameStr.Split("/").Length > 1)
            {
                int indexOfLastSlash = resourceNameStr.LastIndexOf('/');
                return resourceNameStr.Substring(indexOfLastSlash + 1);
            }
            else
            {
                return resourceNameStr;
            }
        }
        private static void validateFileName(string resourceName)
        {
            foreach (char ch in resourceName)
            {
                // @TODO use regex?
                if (!"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_-.".Contains(ch))
                {
                    throw new ArgumentProcessException($"Invalid character in file name: \"{ch}\"");
                }
            }
        }
   
    private static ResourceType inferResourceTypeFromResourceName(string resourceName)
        {
            try{
                if (FileTypeInference.isCodeFileName(resourceName)) return ResourceType.Code;
                if (FileTypeInference.isDataFileName(resourceName)) return ResourceType.Data;
                if (FileTypeInference.isModelFileName(resourceName)) return ResourceType.Model;
                throw new System.Exception();
            }
            catch (System.Exception)
            {
                throw new ArgumentProcessException($"Could not infer resource type from extension of {resourceName}");
            }
        }

        private static void validateVersionString(string resourceStr, string version)
        {
            string[] versionSections = version.Split('.');
            foreach (string section in versionSections)
            {
                if (section.Trim() != section)
                {
                    throw new ArgumentProcessException("Version cannot contain spaces");
                }
                // if this is section empty
                if (section.Length == 0)
                {
                    // if this is also the only section
                    if (versionSections.Length == 1)
                    {
                        throw new ArgumentProcessException($"\"{resourceStr}\" is missing version");
                    }
                    else
                    {
                        throw new ArgumentProcessException(
                            $"Invalid version \"{version}\" in \"{resourceStr}\": Version should be " +
                            "series of numbers separated by periods, like 1.2.3 or 333.3.20"
                        );
                    }
                }
                foreach (char ch in section)
                {
                    if (!"1234567890".Contains(ch))
                    {
                        throw new ArgumentProcessException($"Invalid version character \"{ch}\" in {resourceStr}");
                    }
                }
            }
        }

   
    }

}