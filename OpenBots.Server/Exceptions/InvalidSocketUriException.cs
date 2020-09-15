using System;

namespace OpenBots.Server.Exceptions
{
    /// <summary>
    /// Exception defines Invalid Socket URI passed by user
    /// </summary>
    public class InvalidSocketUriException : Exception
    {
        public InvalidSocketUriException(string message) : base(message)
        {

        }
    }
}
