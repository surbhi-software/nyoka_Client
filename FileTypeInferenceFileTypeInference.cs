
namespace nyoka_Client
{
    public static class FileTypeInference
    {
        public class FileTypeInferenceError : System.Exception
        {
            public FileTypeInferenceError(string message)
            : base(message)
            {
            }
        }

        public static readonly string[] codeFileExtensions = {"py", "ipynb", "r", "jar", "mon"};
        public static readonly string[] modelFileExtensions = {"pmml", "h5", "pb", "pbtxt", "onnx"};
        public static readonly string[] dataFileExtensions = {"json", "csv", "png", "jpg", "jpeg", "zip","txt","md", "webp"};

        private static string getLowerCaseFileExtension(string fileName)
        {
            string[] splitByDot = fileName.Split('.');

            if (splitByDot.Length < 2)
            {
                throw new FileTypeInferenceError($"File name {fileName} has no extension");
            }
            string extension = splitByDot[splitByDot.Length - 1];

            if (extension.Trim().Length == 0)
            {
                throw new FileTypeInferenceError($"Empty file extension {extension}");
            }

            return extension.Trim().ToLower();
        }

        public static bool isCodeFileName(string fileName)
        {
            string extension = getLowerCaseFileExtension(fileName);
            switch(extension)
            {
            case "py":
            case "ipynb":
            case "r":
            case "jar":
            case "mon":
                return true;
            default:
                return false;
            }
        }

        public static bool isModelFileName(string fileName)
        {
            string extension = getLowerCaseFileExtension(fileName);
            switch(extension)
            {
            case "pmml":
            case "h5":
            case "pb":
            case "pbtxt":
            case "onnx" :
                return true;
            default:
                return false;
            }
        }

        public static bool isDataFileName(string fileName)
        {
            string extension = getLowerCaseFileExtension(fileName);
            switch(extension)
            {
            case "json":
            case "csv":
            case "png":
            case "jpg":
            case "jpeg":
            case "zip":
            case "txt":
            case "md":
            case "webp":
                return true;
            default:
                return false;
            }
        }
    }
}
