
using System.Collections.Generic;
using System.Linq;
using static nyoka_Client.Constants;

namespace nyoka_Client
{
    internal class ParseUtils
    {
        public class ArgumentProcessException : System.Exception
        {
            public ArgumentProcessException(string mssg)
            : base (mssg)
            {
            }
        }
        public static ResourceType parseResourceType(string type)
        {
            if (type.ToLower() == "model") return ResourceType.Model;
            if (type.ToLower() == "models") return ResourceType.Model;
            else if (type.ToLower() == "data") return ResourceType.Data;
            else if (type.ToLower() == "code") return ResourceType.Code;
            else throw new ArgumentProcessException($"Invalid resource type \"{type}\"");
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
    }
}