using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.V1
{
    public class ClientSocketTask : AbstractClientSocketTask
    {
        public int AutoReconnectTimeout { get; set; }
        protected string mServerAddress;// { get; set; }
        protected int mServerPort;// { get; set; }

        private AutoResetEvent mSemaphoreCounter = null;
        private Socket mSock;
        #region Constructor
        public ClientSocketTask(string ip, int port) : this(null, ip, port)
        {
        }

        public ClientSocketTask(ISynchronizeInvoke control, string ip, int port) : base(control, null)
        {
            AutoReconnectTimeout = 2000;
            mServerAddress = ip;
            mServerPort = port;
            mSemaphoreCounter = new AutoResetEvent(false);
        }

        public override string GetTag()
        {
            if (String.IsNullOrEmpty(base.GetTag()))
                return String.Format("{0}:{1}", mServerAddress, mServerPort);

            return String.Format("{0}-{1}:{2}", base.GetTag(), mServerAddress, mServerPort);
        }

        public override string GetRemoteAddress()
        {
            return mServerAddress;
        }

        //public ClientSocketTask(ISynchronizeInvoke control) : this("", 0, control)
        //{
        //}
        #endregion
        public void connect(string ip, int port)
        {
            if (ip == mServerAddress && port == mServerPort)
            {
                System.Diagnostics.Trace.WriteLine("connect() no changed...");
                return;
            }

            mServerAddress = ip;
            mServerPort = port;

            lock (mSemaphoreCounter)
            {
                if (mSock != null && mSock.Connected)
                {
                    //mSock.Disconnect(true);
                    mSock.Close();
                    mSock = null;
                }
            }

            mSemaphoreCounter.Set();
        }

        protected override void onCanceled()
        {
            mSemaphoreCounter.Set();
            base.onCanceled();

            lock (mSemaphoreCounter)
            {
                if (mSock != null)
                {
                    mSock.Close();
                }
            }
            mSemaphoreCounter.Set();
        }

        public override Socket GetSocket()
        {
            return mSock;
        }

        public override bool isConnected()
        {
            lock (mSemaphoreCounter)
            {
                if (mSock == null)
                    return false;

                if (mSock.Connected)
                    return true;
            }

            return false;
        }

        protected virtual bool checkConnected(Socket sock, int timeout)
        {
            if (!sock.Connected)
            {
                IAsyncResult result = null;
                try
                {
                    if (mServerPort == 0)
                    {
                        return false;
                    }

                    IPEndPoint ipe = null;
                    if (String.IsNullOrEmpty(mServerAddress))
                        ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), mServerPort);
                    else
                        ipe = new IPEndPoint(IPAddress.Parse(mServerAddress), mServerPort);

#if DEBUG
                    long startTime = DateTime.Now.Ticks;
                    if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LocalClientTask::checkConnected(1) connect timeout:{0} time:{1}",
                                timeout, startTime / TimeSpan.TicksPerSecond));
                    }

#endif

#if true
                    result = sock.BeginConnect(ipe, null, null);

                    bool success = result.AsyncWaitHandle.WaitOne(timeout, true);
#if DEBUG
                    if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LocalClientTask::checkConnected(3) connect to port:{0}, result:{1}, Connected:{2}, waitResult:{3}, timeout:{4}, time:{5}",
                                mServerPort, result, sock.Connected, success, timeout, new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(startTime)).Duration().TotalMilliseconds));
                    }

#endif
                    try
                    {
                        sock.EndConnect(result);
                    }
                    catch (Exception)
                    {
                    }

                    result = null;
#else
                    sock.Connect(ipe);
#endif
#if DEBUG
                    if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LocalClientTask::checkConnected(3) connect to port : {0} result:{1}",
                                mServerPort, sock.Connected));
                    }

#endif
                    if (!IsCanceled())
                    {
                        if (sock.Connected)
                        {
                            notifyConnectedListener();
                        }
                    }
                }
                //catch (ObjectDisposedException ex)
                //{
                //    Cancel(true);

                //    if (EnableDebug)
                //    {
                //        System.Diagnostics.Trace.WriteLine(String.Format("ClientSocketTask::checkConnected(7) connect to port : {0}, iscancel:{1} failed:{2}",
                //        mServerPort, isCanceled(), ex));
                //    }          
                //}
                catch (Exception ex)
                {
#if DEBUG
                    if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("ClientSocketTask::checkConnected(8) connect to port : {0}, iscancel:{1} failed:{2}",
                        mServerPort, IsCanceled(), ex));
                    }
#endif
                }
                finally
                {
#if DEBUG				
                    if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("ClientSocketTask::checkConnected(9) connect to port : {0}, iscancel:{1} ",
                        mServerPort, IsCanceled()));
                    }
#endif
                    try
                    {
                        if (result != null)
                            sock.EndConnect(result);
                    }
                    catch (Exception)
                    {

                    }
                }

                return sock.Connected;
            }

            return sock.Connected;
        }

        protected override bool onReadDataError(Socket sock)
        {
            //Logger.Info(String.Format("{0}::onReadDataError, {1}", getTag(), sock.Connected));

            // for client connect to server...
            sock.Disconnect(true);
            sock.Close();

            notifyDisconnectedListener();

            lock (mSemaphoreCounter)
            {
                mSock = null;
            }

            return true;
        }

        protected override void doProc(object param)
        {
            //Socket sock =  
            System.Diagnostics.Trace.WriteLine(">>>>>>>>>>>>>>LocalClientTask()>>>>>>>>>>>>>>>>>");
            if (mSock == null)
            {
                mSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                onSocketCreated(mSock);
            }

            byte[] recv_buffer = new byte[1024];
            //int recv_count = 0;
            Socket sock = mSock;
            try
            {
                while (!IsCanceled())
                {
                    try
                    {
                        lock (mSemaphoreCounter)
                        {
                            if (mSock == null)
                            {
                                mSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                                onSocketCreated(mSock);

                                sock = mSock;
                            }
                        }

                        if (!checkConnected(mSock, 200))
                        {
                            mSemaphoreCounter.WaitOne(Math.Max(1000, AutoReconnectTimeout));
                            continue;
                        }


                        CheckWriteOrReadBuffer(mSock, recv_buffer);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        lock (mSemaphoreCounter)
                        {
                            mSock = null;
                        }

                        notifyDisconnectedListener();
                        if (EnableDebug)
                            System.Diagnostics.Trace.WriteLine("LocalClientTask::doProc remote socket closed:" + ex);
                    }
                    catch (SocketException ex)
                    {
                        if (EnableDebug)
                            System.Diagnostics.Trace.WriteLine("LocalClientTask::sendOrRecvData SocketException exception:" + ex);

                        if ((ex.SocketErrorCode > SocketError.WouldBlock
                            && ex.SocketErrorCode < SocketError.IsConnected)
                            || ex.SocketErrorCode > SocketError.IsConnected)
                        {
                            lock (mSemaphoreCounter)
                            {
                                mSock = null;
                            }

                            notifyDisconnectedListener();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (EnableDebug)
                            System.Diagnostics.Trace.WriteLine("LocalClientTask::sendOrRecvData Receive exception:" + ex);
                    }

                }

            }
            catch (Exception e)
            {
                if (EnableDebug)
                    System.Diagnostics.Trace.WriteLine("LocalClientTask::doproc cause exception:" + e);
                //throw;
            }

            lock (mSemaphoreCounter)
            {
                mSock = null;
            }

            sock.Close();

            notifyDisconnectedListener();

            System.Diagnostics.Trace.WriteLine("<<<<<<<<<<<<<<<<<LocalClientTask()<<<<<<<<<<<<<<");
        }
    }
}
