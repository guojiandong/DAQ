using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Args
{
    [Guid("6A8B98F1-516B-4200-804E-C3B9A119F254")]
    public class CommandEventArgs<T> : ObjectEventArgs
    {
        /// <summary>
        /// 命令
        /// </summary>
        public readonly T Command;

        public CommandEventArgs(T cmd, object v) : base(v)
        {
            this.Command = cmd;
        }
    }
}
