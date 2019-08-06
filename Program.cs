using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace nyoka_Client
{
    class Program
    {
        static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<InitOptions, AddOptions, RemoveOptions,
            ListOptions>(args.ToList())
              .MapResult(
                (InitOptions opts) => InitOptions.tryCreateDirIfNonExistent(),
                (AddOptions opts) => AddOptions.AddPackage(opts),
                (RemoveOptions opts) => RemoveOptions.RemovePackage(opts),
                //(ListOptions opts) => ListOptions.listResources(),
                errs => 1);
        }
    }
}
