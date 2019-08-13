using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using System.Reflection;
namespace nyoka_Client
{
    public  class GetOptions
    {
        
        private static Dictionary<string, string> AllOptions = new Dictionary<string, string>();
        public static void GetAllOptions ()
        {
            var verbTypes = (from type in Assembly.GetExecutingAssembly().GetTypes()
              let testAttribute = Attribute.GetCustomAttribute(type, typeof(VerbAttribute))
                              where testAttribute != null                               
                              select type).ToList();
            foreach(var item in verbTypes)
            {
                AllOptions.Add(item.CustomAttributes.ToList()
              .FirstOrDefault().ConstructorArguments.ToList().FirstOrDefault().Value.ToString(),item.CustomAttributes.ToList()
              .FirstOrDefault().NamedArguments.ToList().FirstOrDefault().TypedValue.ToString());
            }
            PrintTable table = new PrintTable {
                {"Action Name", 0},
                {"Action Description", 0},
            };
             foreach (KeyValuePair<string, string> descriptionKVPair in AllOptions)
            {
                string actionName = descriptionKVPair.Key;
                string description = descriptionKVPair.Value;

                table.addRow(
                    actionName,
                    description
                );
            }
            Logger.logTable(table, visibleLines: false);
        }
    }
}