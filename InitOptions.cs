using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    [Verb("init", HelpText = "Initialize code, data and model folders.")]
    public class InitOptions
    {
        
        internal static void tryCreateDirIfNonExistent()
        {
            try
            {
                foreach (var di in Constants.dirNames)
                {
                    if (Directory.Exists(di))
                    {
                        Logger.logLine($"Directory \"{di}\" already exists");
                    }
                    else
                    {
                        Directory.CreateDirectory(di);
                          Logger.logLine($"Directory \"{di}\" created");
                        Directory.CreateDirectory(Path.Join(di, Constants.nyokaFolderName));
                        Logger.logLine($"Directory \"{Path.Join(di, Constants.nyokaFolderName)}\" created");
                    }
                }
            }
            catch (System.Exception)
            {
                Logger.logLine($"Failed to create directory \"{Constants.dirNames}\"");
            }
        }

        public static int initDirectories()
        {
            try
            {
                tryCreateDirIfNonExistent();
                return 1;
            }
            catch (Exception ex)
            {
                Logger.logError($"File System Error: " + ex.Message);
                return 0;
            }
        }
    }
}