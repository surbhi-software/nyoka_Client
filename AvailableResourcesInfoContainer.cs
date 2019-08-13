using System.Collections.Generic;
using Newtonsoft.Json;
public class AvailableResourcesInfoContainer
    {
        
        public class AvailableResourceDescription
        {
            public long byteCount;
            public string versionStr;

            public AvailableResourceDescription(long byteCount, string versionStr)
            {
                this.byteCount = byteCount;
                this.versionStr = versionStr;
            }
        }
        public static AvailableResourcesInfoContainer deserialize(string str)
        {
            return JsonConvert.DeserializeObject<AvailableResourcesInfoContainer>(str);
        }

        public Dictionary<string, AvailableResourceDescription> resourceDescriptions;
        public AvailableResourcesInfoContainer(Dictionary<string, AvailableResourceDescription> resourceDescriptions)
        {
            this.resourceDescriptions = resourceDescriptions;
        }

        public AvailableResourcesInfoContainer()
        {
            this.resourceDescriptions = new Dictionary<string, AvailableResourceDescription>();
        }

        public string serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
