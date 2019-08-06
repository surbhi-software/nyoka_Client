using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    [Verb("list", HelpText = "List resources in local files.")]
    class ListOptions
    {
        private static int hasNecessaryDirs()
        {
            try
            {
                foreach (string dirName in Constants.dirNames)
                {
                    if (!Directory.Exists(dirName))
                    {
                        return 0;
                    }

                    if (!Directory.Exists(Path.Join(dirName, Constants.nyokaFolderName)))
                    {
                        return 0;
                    }
                }

                return 1;
            }
            catch (System.Exception)
            {
                return 0;
                //throw new FSOpsException("Failed to check for necessary directories and files");
            }
        }
        public static void listResources(Constants.ResourceType? listType)
        {
            try
            {
                if (hasNecessaryDirs() == 0)
                {
                    Logger.logError($"Missing some or all resource directories in current directory. Try running {Constants.APPLICATION_ALIAS} init?");
                    return;
                }
            }
            catch
            { }

        }
    }
}