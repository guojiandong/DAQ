using System.Net;
using System.Linq;
using System;
using System.Net.Sockets;
using Ksat.AppPlugIn.Exceptions;

namespace Ksat.AppPlugIn.Utils
{
    /// <summary>
    /// Provides utility methods.
    /// </summary>
    public static class NetUtils
    {
        /// <summary>
        /// Gets an <see cref="IPAddress"/> from an IP or a host string.
        /// </summary>
        /// <param name="ipOrHost">IP or Host address.</param>
        /// <returns>Parsed <see cref="IPAddress"/>.</returns>
        public static IPAddress GetIpAddress(string ipOrHost)
        {
#if NET40
            return null;
#else
            string host = Dns.GetHostAddressesAsync(ipOrHost).Result.First().ToString();

            IPAddress address = null;
            if (IPAddress.TryParse(host, out address))
                return address;

            return null;
#endif
        }

        /// <summary>
        /// Creates a new <see cref="IPEndPoint"/> with an IP or host and a port number.
        /// </summary>
        /// <param name="ipOrHost">IP or Host address.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        public static IPEndPoint CreateIpEndPoint(string ipOrHost, int port)
        {
            IPAddress address = GetIpAddress(ipOrHost);

            if (address == null)
                throw new ConfigurationException($"Invalid host or ip address: {ipOrHost}.");
            if (port <= 0)
                throw new ConfigurationException($"Invalid port: {port}");

            return new IPEndPoint(address, port);
        }

        /// <summary>
        /// Gets the buffer at the offset and size passed.
        /// </summary>
        /// <param name="bufferSource">Input buffer source</param>
        /// <param name="offset">Data offset</param>
        /// <param name="size">Data size</param>
        /// <returns></returns>
        public static byte[] GetPacketBuffer(byte[] bufferSource, int offset, int size)
        {
            var buffer = new byte[size];

            Buffer.BlockCopy(bufferSource, offset, buffer, 0, size);

            return buffer;
        }

        /// <summary>
        /// Creates a new <see cref="SocketAsyncEventArgs"/> instance.
        /// </summary>
        /// <param name="userToken">User token</param>
        /// <param name="completedAction">Completed operation action</param>
        /// <returns></returns>
        public static SocketAsyncEventArgs CreateSocketAsync(object userToken, EventHandler<SocketAsyncEventArgs> completedAction)
        {
            var socketAsync = new SocketAsyncEventArgs
            {
                UserToken = userToken
            };

            socketAsync.Completed += completedAction;

            return socketAsync;
        }

        /// <summary>
        /// Creates a new <see cref="SocketAsyncEventArgs"/> instance.
        /// </summary>
        /// <param name="userToken">User token</param>
        /// <param name="completedAction">Completed operation action</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <returns></returns>
        public static SocketAsyncEventArgs CreateSocketAsync(object userToken, EventHandler<SocketAsyncEventArgs> completedAction, int bufferSize)
        {
            SocketAsyncEventArgs socketAsync = CreateSocketAsync(userToken, completedAction);

            if (bufferSize > 0)
                socketAsync.SetBuffer(new byte[bufferSize], 0, bufferSize);

            return socketAsync;
        }

        /// <summary>
        /// -1 : offline, else ms
        /// </summary>
        /// <param name="ip">目标地址</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static int CheckOnline(string ip, int timeout = 20000)
        {
            //Ping 实例对象;
            try
            {
                System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
                //ping选项;
                System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
                //options.DontFragment = true; //数据包是否分段

                byte[] buf = System.Text.Encoding.ASCII.GetBytes("ping");

                //调用同步send方法发送数据，结果存入reply对象;
                System.Net.NetworkInformation.PingReply reply = pingSender.Send(ip, timeout, buf, options);

                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return (int)reply.RoundtripTime;
                }
            }
            catch
            {
            }

            return -1;
        }
    }
}
