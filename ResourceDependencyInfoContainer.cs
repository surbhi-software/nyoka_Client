using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace nyoka_Client
{
public class ResourceDependencyInfoContainer
    {
        public class DependencyDescription
        {
            public bool isDirectDependency;
            public string versionStr;
            public long byteCount;

            public DependencyDescription(string versionStr, bool isDirectDependency, long byteCount)
            {
                this.versionStr = versionStr;
                this.isDirectDependency = isDirectDependency;
                this.byteCount = byteCount;
            }
        }

        public static ResourceDependencyInfoContainer deserialize(string str)
        {
            return JsonConvert.DeserializeObject<ResourceDependencyInfoContainer>(str);
        }

        public Dictionary<string, DependencyDescription> codeDeps;
        public Dictionary<string, DependencyDescription> dataDeps;
        public Dictionary<string, DependencyDescription> modelDeps;

        public ResourceDependencyInfoContainer(
            Dictionary<string, DependencyDescription> codeDeps,
            Dictionary<string, DependencyDescription> dataDeps,
            Dictionary<string, DependencyDescription> modelDeps)
        {
            this.codeDeps = codeDeps;
            this.dataDeps = dataDeps;
            this.modelDeps = modelDeps;
        }

        public ResourceDependencyInfoContainer()
        {
            this.codeDeps = new Dictionary<string, DependencyDescription>();
            this.dataDeps = new Dictionary<string, DependencyDescription>();
            this.modelDeps = new Dictionary<string, DependencyDescription>();
        }

        public string serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}