using System;

namespace Ksat.AppPlugIn.Exceptions
{
    /// <summary>
    /// Represents a generic base exception.
    /// </summary>
    public class BaseException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="BaseException"/>.
        /// </summary>
        /// <param name="message"></param>
        public BaseException(string message)
            : base(message, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="BaseException"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
