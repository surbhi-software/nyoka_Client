using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    [Verb("add", HelpText = "Download and add a resource file from repository server to local files/zementis server/zementis modeler.")]
    class AddOptions : IOptions
    {
        [Option("alias", HelpText = "(Optional argument.)")]
        string Alias { get; set; }
        public string ResourceName { get ; set ; }

        public static int AddPackage(AddOptions opts)
        {
            return 1;
            //Constants.ResourceType resourceType = resourceDescription.resourceType;
            //string resourceName = resourceDescription.resourceName;
        }
    }
}