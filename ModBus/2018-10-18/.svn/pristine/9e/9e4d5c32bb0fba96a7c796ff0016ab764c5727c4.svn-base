using Ksat.AppPlugIn.Model.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Cmd
{
    [Guid("DFACEB7D-4A42-448C-863A-6703C2D2791F")]
    public class RequestCommandEventArgs<TCommand> : AbstractCommandEventArgs
    {
        /// <summary>
        /// 命令
        /// </summary>
        public readonly TCommand Command;

        public RequestCommandEventArgs(TCommand cmd, object v) : base(true, v)
        {
            this.Command = cmd;
        }
    }


    #region string command
    public class RequestStringCommandEventArgs : RequestCommandEventArgs<string>
    {
        public RequestStringCommandEventArgs(string cmd, object v) : base(cmd, v)
        {
        }
    }

    [Guid("3124D2A9-3BCA-4704-BF42-26CAEE0D1800")]
    public class RequestStringCommandWithFilterEventArgs : RequestStringCommandEventArgs
    {
        public const int ALL_FILTER_KEY = -1;

        public readonly int Filter;

        public RequestStringCommandWithFilterEventArgs(string cmd, int filter, object v) : base(cmd, v)
        {
            this.Filter = filter;
        }
    }

    [Guid("3815B190-7012-418E-8516-3D9A0C3BE3EC")]
    public class RequestSyncStringCommandEventArgs : RequestStringCommandEventArgs
    {
        public readonly AutoResetEvent WaitEvent;

        public object Result { get; set; }

        public RequestSyncStringCommandEventArgs(string cmd, object v) : base(cmd, v)
        {
            this.WaitEvent = new AutoResetEvent(true);
            this.Result = null;
        }

        public void Wait()
        {
            Wait(-1);
        }

        public void Wait(int timeout)
        {
            if (timeout > 0)
                this.WaitEvent.WaitOne(timeout);
            else
                this.WaitEvent.WaitOne();
        }

        public virtual void Release()
        {
            this.WaitEvent.Set();
        }

        public virtual void Lock()
        {
            this.WaitEvent.Reset();
        }
    }
    #endregion

    #region int command
    public class RequestIntCommandEventArgs : RequestCommandEventArgs<int>
    {
        public RequestIntCommandEventArgs(int cmd, object v) : base(cmd, v)
        {
        }
    }


    public class RequestIntCommandWithFilterEventArgs : RequestIntCommandEventArgs
    {
        public const int ALL_FILTER_KEY = -1;

        public readonly int Filter;

        public RequestIntCommandWithFilterEventArgs(int cmd, int filter, object v) : base(cmd, v)
        {
            this.Filter = filter;
        }
    }
    
    public class RequestSyncCommandEventArgs<TCommand> : RequestCommandEventArgs<TCommand>
    {
        public readonly AutoResetEvent WaitEvent;

        public object Result { get; set; }

        public RequestSyncCommandEventArgs(TCommand cmd, object v) : base(cmd, v)
        {
            this.WaitEvent = new AutoResetEvent(true);
            this.Result = null;
        }

        public void Wait()
        {
            Wait(-1);
        }

        public void Wait(int timeout)
        {
            if (timeout > 0)
                this.WaitEvent.WaitOne(timeout);
            else
                this.WaitEvent.WaitOne();
        }

        public virtual void Release()
        {
            this.WaitEvent.Set();
        }

        public virtual void Lock()
        {
            this.WaitEvent.Reset();
        }
    }
    #endregion
}
