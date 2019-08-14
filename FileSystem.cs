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
    public class FileSystem
    {
        public static FileStream createResourceFile(ResourceType resourceType, string resourceName)
        {
            try
            {
                return File.Create(Path.Join(resourceDirPath(resourceType), resourceName));
            }
            catch (System.Exception)
            {
                throw new Exception($"Failed to create file for {resourceType.ToString().ToLower()} resource {resourceName}");
            }
        }

        

    }
}