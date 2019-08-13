namespace nyoka_Client
{
    public static class Constants
    {
        public enum ResourceType { Code, Data, Model }
        public static readonly string codeDirName = "Code";
        public static readonly string dataDirName = "Data";
        public static readonly string modelDirName = "Models";
        public static readonly string nyokaFolderName = ".nyoka";
        public static readonly string nyokaVersionExtension = ".version";
        public static readonly string[] dirNames = new string[] { codeDirName, dataDirName, modelDirName };
        public static System.ConsoleColor warningColor = System.ConsoleColor.Yellow;
        public static System.ConsoleColor errorColor = System.ConsoleColor.Red;
        public static System.ConsoleColor tableHeaderColor = System.ConsoleColor.Yellow;
        public static System.ConsoleColor tableFrameColor = System.ConsoleColor.White;
        public static System.ConsoleColor questionColor = System.ConsoleColor.White;

        internal const string RESOURCE_TYPE_HINT = "Resource Type: \"code\", \"data\" or \"model\"";
        internal const string APPLICATION_ALIAS = "nyoka";

        internal const string HeaderStringType = " Type";
        internal const string HeaderStringNameOfResource = " Name of Resource";
        internal const string HeaderStringLatestVersion = " Latest Version";
        internal const string HeaderStringLocalVersion = " Local Version";
        internal const string HeaderStringFileSize = " File Size";
        internal const string HeaderStringVersion = " Version";
        public static readonly string remoteServerConfigFileName = "nyokaremote.json";
        public static string resourceDirPath(ResourceType resourceType)
        {
            string strReturn = "";
            if (resourceType == ResourceType.Code)
                return codeDirName;
            if (resourceType == ResourceType.Data)
                return dataDirName;
            if (resourceType == ResourceType.Model)
                return modelDirName;

            Logger.logLine($"No matching resource directory for resource type {resourceType}");
            return strReturn;
        }
    }
}
