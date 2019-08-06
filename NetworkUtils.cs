
using System.Collections.Generic;

using System.Linq;


namespace NetworkUtilsNS
{
    public static class NetworkUtils
    {
        public class NetworkUtilsException : System.Exception
        {
            public NetworkUtilsException(string mssg)
            : base(mssg)
            {
            }
        }
    }
}