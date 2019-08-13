using System;
using JsonPitCore;


namespace nyoka_Client
{
    public class NyokaRemoteInfo : JsonPitCore.Item
    {
        public string RepositoryServer { get; set; }
        public string ZementisServer { get; set; }
        public string ZementisModeler { get; set; } 
    }
}