
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Communicate.FastIocp.Base;

namespace Ksat.AppPlugIn.Threading.Task.Sock
{
    public class SocketItemEventArgs : Model.Args.GenericEventArgs<Communicate.FastIocp.Base.IConnection>
    {
        /// <summary>
        /// 创建的时间
        /// </summary>
        public readonly long TimeTicks;

        public SocketItemEventArgs(Communicate.FastIocp.Base.IConnection v) : base(v)
        {
            this.TimeTicks = Utils.DateHelper.ToMillisecondsSinceEpoch(DateTime.UtcNow);
        }
    }

    public class SocketCustomRequestEventArgs : SocketItemEventArgs
    {
        public readonly string Command;

        public readonly object UserData;

        public SocketCustomRequestEventArgs(string cmd, object data, IConnection v = null) : base(v)
        {
            this.Command = cmd;
            this.UserData = data;
        }
    }

    public class SocketConnectedEventArgs : SocketItemEventArgs
    {
        public SocketConnectedEventArgs(Communicate.FastIocp.Base.IConnection v) : base(v)
        {
        }
    }


    public class SocketDisconnectedEventArgs : SocketItemEventArgs
    {
        public readonly Exception Ex;

        public SocketDisconnectedEventArgs(Communicate.FastIocp.Base.IConnection v, Exception ex) : base(v)
        {
            this.Ex = ex;
        }
    }

    public class SocketExceptionEventArgs : SocketItemEventArgs
    {
        public readonly Exception Ex;

        public SocketExceptionEventArgs(Communicate.FastIocp.Base.IConnection v, Exception ex) : base(v)
        {
            this.Ex = ex;
        }
    }

    public class SocketSendCallbackEventArgs : SocketItemEventArgs
    {
        public readonly Communicate.FastIocp.Base.Packet Packet;
        public readonly bool Success;

        public SocketSendCallbackEventArgs(Communicate.FastIocp.Base.IConnection v, Communicate.FastIocp.Base.Packet pkt, bool success) : base(v)
        {
            this.Packet = pkt;
            this.Success = success;
        }
    }

    public class SocketRecvDataEventArgs<TMessage> : SocketItemEventArgs
    {
        public readonly TMessage Msg;

        public SocketRecvDataEventArgs(Communicate.FastIocp.Base.IConnection v, TMessage msg) : base(v)
        {
            this.Msg = msg;
        }
    }
}
