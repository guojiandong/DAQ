using System;
namespace Ksat.AppPlugIn.Exceptions
{
    /// <summary>
    /// Represents an Network client not found exception.
    /// </summary>
    public class ClientNotFoundException : BaseException
    {
        /// <summary>
        /// Creates a new <see cref="ClientNotFoundException"/>.
        /// </summary>
        /// <param name="clientId">Client Unique Id</param>
        public ClientNotFoundException(Guid clientId)
            : base($"Cannot found client {clientId}.")
        {
        }

        /// <summary>
        /// Creates a new <see cref="ClientNotFoundException"/>.
        /// </summary>
        /// <param name="message"></param>
        public ClientNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ClientNotFoundException"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ClientNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
