using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Server
{
    /// <summary>
    /// socket listener
    /// </summary>
    public sealed class SocketListener : ISocketListener
    {
        #region Private Members
        private readonly Base.ISession _host = null;
        private const int BACKLOG = 1500;
        private Socket _socket = null;
        private readonly SocketAsyncEventArgs _ae = null;
        #endregion

        #region Constructors
        /// <summary>
        /// new
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="host"></param>
        /// <exception cref="ArgumentNullException">endPoint is null</exception>
        /// <exception cref="ArgumentNullException">host is null</exception>
        public SocketListener(IPEndPoint endPoint, Base.ISession host)
        {
            if (endPoint == null) throw new ArgumentNullException("endPoint");
            if (host == null) throw new ArgumentNullException("host");

            this.EndPoint = endPoint;
            this._host = host;

            this._ae = new SocketAsyncEventArgs();
            this._ae.Completed += this.AcceptCompleted;
        }
        #endregion

        #region ISocketListener Members
        /// <summary>
        /// socket accepted event
        /// </summary>
        public event Action<ISocketListener, Base.IConnection> Accepted;
        /// <summary>
        /// get listener endPoint
        /// </summary>
        public EndPoint EndPoint { get; set; }

        public bool IsStarted()
        {
            if(this._socket != null)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// start
        /// </summary>
        public void Start()
        {
            if (this._socket == null)
            {
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._socket.Bind(this.EndPoint);
                this._socket.Listen(BACKLOG);

                this.AcceptAsync(this._socket);
            }
        }
        /// <summary>
        /// stop
        /// </summary>
        public void Stop()
        {
            if (this._socket != null)
            {
                this._socket.Close();
                this._socket = null;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// accept socket.
        /// </summary>
        /// <param name="socket"></param>
        private void AcceptAsync(Socket socket)
        {
            if (socket == null) return;

            bool completed = true;
            try { completed = this._socket.AcceptAsync(this._ae); }
            catch (Exception ex) { Logging.Logger.Error(this.GetType().Name, ex.Message, ex); }

            if (!completed) ThreadPool.QueueUserWorkItem(_ => this.AcceptCompleted(this, this._ae));
        }
        /// <summary>
        /// async accept socket completed handle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket accepted = null;
            if (e.SocketError == SocketError.Success) accepted = e.AcceptSocket;
            e.AcceptSocket = null;

            if (accepted != null)
            {
                Base.IConnection connection = this._host.NewConnection(accepted);
                connection.Tag = ((IPEndPoint)accepted.RemoteEndPoint).ToString();
                this.Accepted(this, connection);
            }
                

            //continue to accept!
            this.AcceptAsync(this._socket);
        }
        #endregion
    }
}