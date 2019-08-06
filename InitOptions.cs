using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
   [Verb("init", HelpText = "Initialize code, data and model folders." )]
	class InitOptions 
	{
       public static int tryCreateDirIfNonExistent()
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
                    }
                }
            return 1;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
	}
}