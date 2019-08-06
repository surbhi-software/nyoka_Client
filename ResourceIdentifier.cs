using nyoka_Client;
namespace nyoka_Client
{
    public class ResourceIdentifier
    {
        public string resourceName;
        public string version;
        public Constants.ResourceType resourceType;

        public ResourceIdentifier(string resourceName, Constants.ResourceType resourceType)
        {
            this.resourceName = resourceName;
            this.resourceType = resourceType;
            this.version = null;
        }
        public ResourceIdentifier(string resourceName, Constants.ResourceType resourceType, string version)
        {
            this.resourceName = resourceName;
            this.resourceType = resourceType;
            this.version = version;
        }
    }
}