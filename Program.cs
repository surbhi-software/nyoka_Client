﻿using System;
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
        //private static Constants.ResourceType? listType;   
        static int Main(string[] args)
        {
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                // settings.IgnoreUnknownArguments = false;
            });
            PrintTable table = new PrintTable {
                    {Constants.HeaderStringType, 6},
                    {Constants.HeaderStringNameOfResource, 21},
                    {Constants.HeaderStringVersion, 16},
                    {Constants.HeaderStringFileSize, 11},
                };
            if (args.ToList().Count == 0)
            {
                Logger.logError($"{Constants.APPLICATION_ALIAS} must be called with an action name. Available actions:");
                GetOptions.GetAllOptions();
                return 0;
            }

            return CommandLine.Parser.Default.ParseArguments<InitOptions, AddOptions, ListOptions, AvailableOptions,
             DependencyOption, PublishOptions, RemoveOptions>(args.ToList())
              .MapResult(
                (InitOptions opts) => InitOptions.initDirectories(),
                (AddOptions opts) => AddOptions.AddPackage(args.ToList()),
                (ListOptions opts) => ListOptions.ListResources(opts.resourceType, args.ToList()),
                (AvailableOptions opts) => AvailableOptions.Available(),
                 (DependencyOption opts) => DependencyOption.ListAllDependencies(args.ToList()),
                (PublishOptions opts) => PublishOptions.Publish(args.ToList()),
                (RemoveOptions opts) => RemoveOptions.RemovePackage(args.ToList()),
                errs => 1);
        }
    }
}