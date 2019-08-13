using System.Collections.Generic;

using System.Linq;
using nyoka_Client;
using Newtonsoft.Json;
using System.IO;

namespace nyoka_Client
{
 public class ResourceVersionsInfoContainer
    {
        public class ResourceVersionDescription
        {
            public long byteCount;
            public ResourceVersionDescription(long byteCount)
            {
                this.byteCount = byteCount;
            }
        }
        
        public static ResourceVersionsInfoContainer deserialize(string str)
        {
            return JsonConvert.DeserializeObject<ResourceVersionsInfoContainer>(str);
        }

        public Dictionary<string, ResourceVersionDescription> versions;

        public string latestVersion;

        public ResourceVersionsInfoContainer(Dictionary<string, ResourceVersionDescription> versions, string latestVersion)
        {
            this.versions = versions;
            this.latestVersion = latestVersion;
        }

        public string serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}