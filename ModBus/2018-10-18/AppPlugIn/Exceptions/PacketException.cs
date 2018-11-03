using System;
namespace Ksat.AppPlugIn.Exceptions
{
    /// <summary>
    /// Represents an Network packet exception.
    /// </summary>
    public class PacketException : BaseException
    {
        /// <summary>
        /// Creates a new <see cref="PacketException"/>.
        /// </summary>
        /// <param name="message"></param>
        public PacketException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PacketException"/> with an inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PacketException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
