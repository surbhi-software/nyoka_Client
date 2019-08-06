using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    [Verb("remove", 
    HelpText = "Removes a code resource named programName.py from the code folder in the current directory.")]
    class RemoveOptions : IOptions
    {
        public string Alias { get ; set ; }
        public string ResourceName { get ; set ; }

        public static int RemovePackage(RemoveOptions opts)
        {
            return 1;
            //Constants.ResourceType resourceType = resourceDescription.resourceType;
            //string resourceName = resourceDescription.resourceName;
        }
    }
}