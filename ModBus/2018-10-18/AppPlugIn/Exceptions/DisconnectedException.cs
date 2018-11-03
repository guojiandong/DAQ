using System;
namespace Ksat.AppPlugIn.Exceptions
{
    /// <summary>
    /// Represents an Network disconnected exception.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when a client is disconnected from the server.
    /// </remarks>
    public class DisconnectedException : BaseException
    {
        /// <summary>
        /// Creates a new <see cref="DisconnectedException"/> instance.
        /// </summary>
        public DisconnectedException()
            : this("Disconnected")
        {
        }

        /// <summary>
        /// Creates a new <see cref="DisconnectedException"/> instance.
        /// </summary>
        /// <param name="message"></param>
        public DisconnectedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DisconnectedException"/> instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DisconnectedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
