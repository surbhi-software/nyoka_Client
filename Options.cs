using System.Collections.Generic;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    interface IOptions
    {
        [Value(0, MetaName = "resource name",
            HelpText = "resource file to be processed.",
            Required = true)]
        string ResourceName { get; set; }
    }
}