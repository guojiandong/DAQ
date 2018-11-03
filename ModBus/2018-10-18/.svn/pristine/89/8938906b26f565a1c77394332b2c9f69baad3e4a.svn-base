using Ksat.AppPlugIn.Model.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Cmd
{
    [Guid("5FA1C508-6EBF-4120-AE34-D711CADC54F5")]
    public class ResponseCommandEventArgs<TCommand> : AbstractCommandEventArgs
    {
        /// <summary>
        /// 命令
        /// </summary>
        public readonly TCommand Command;

        public ResponseCommandEventArgs(TCommand cmd, object v) : base(false, v)
        {
            this.Command = cmd;
        }
    }

    public class ResponseCommandWithFilterEventArgs<TCommand> : ResponseCommandEventArgs<TCommand>
    {
        public readonly int Filter;

        public ResponseCommandWithFilterEventArgs(TCommand cmd, int filter, object v) : base(cmd, v)
        {
            this.Filter = filter;
        }
    }


    public class ResponseStringCommandWithFilterEventArgs : ResponseCommandWithFilterEventArgs<string>
    {
        public ResponseStringCommandWithFilterEventArgs(string cmd, int filter, object v) : base(cmd, filter, v)
        {
        }
    }



    public class ResponseIntCommandWithFilterEventArgs : ResponseCommandWithFilterEventArgs<int>
    {
        public ResponseIntCommandWithFilterEventArgs(int cmd, int filter, object v) : base(cmd, filter, v)
        {
        }
    }
}
