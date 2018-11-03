using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    /// <summary>
    /// <see cref="SocketAsyncEventArgs"/> pool
    /// </summary>
    internal class SocketAsyncEventArgsPool
    {
        #region Private Members
        private readonly int _messageBufferSize;
        private readonly ConcurrentStack<SocketAsyncEventArgs> _pool =
            new ConcurrentStack<SocketAsyncEventArgs>();
        #endregion

        #region Constructors
        /// <summary>
        /// new
        /// </summary>
        /// <param name="messageBufferSize"></param>
        public SocketAsyncEventArgsPool(int messageBufferSize)
        {
            this._messageBufferSize = messageBufferSize;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// acquire
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Acquire()
        {
            SocketAsyncEventArgs e = null;
            if (this._pool.TryPop(out e)) return e;

            e = new SocketAsyncEventArgs();
            e.SetBuffer(new byte[this._messageBufferSize], 0, this._messageBufferSize);
            return e;
        }
        /// <summary>
        /// release
        /// </summary>
        /// <param name="e"></param>
        public void Release(SocketAsyncEventArgs e)
        {
            if (this._pool.Count < 10000)
            {
                this._pool.Push(e);
                return;
            }

            e.Dispose();
        }
        #endregion
    }
}
