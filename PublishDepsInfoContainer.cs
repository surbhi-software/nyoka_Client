using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using static nyoka_Client.Constants;
using Newtonsoft.Json;
namespace nyoka_Client
{
     public class PublishDepsInfoContainer
    {
        public class PublishDepDescription
        {
            public string version;

            public PublishDepDescription(string version)
            {
                this.version = version;
            }
        }

        public Dictionary<string, PublishDepDescription> codeDeps;
        public Dictionary<string, PublishDepDescription> dataDeps;
        public Dictionary<string, PublishDepDescription> modelDeps;

        public static PublishDepsInfoContainer deserialize(string str)
        {
            return JsonConvert.DeserializeObject<PublishDepsInfoContainer>(str);
        }

        public PublishDepsInfoContainer()
        {
            this.codeDeps = new Dictionary<string, PublishDepDescription>();
            this.dataDeps = new Dictionary<string, PublishDepDescription>();
            this.modelDeps = new Dictionary<string, PublishDepDescription>();
        }

        public PublishDepsInfoContainer(
            Dictionary<string, PublishDepDescription> codeDeps,
            Dictionary<string, PublishDepDescription> dataDeps,
            Dictionary<string, PublishDepDescription> modelDeps)
        {
            this.codeDeps = codeDeps;
            this.dataDeps = dataDeps;
            this.modelDeps = modelDeps;
        }

        public string serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}