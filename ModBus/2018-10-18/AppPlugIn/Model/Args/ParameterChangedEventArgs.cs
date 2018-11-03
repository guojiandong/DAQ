using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Args
{
    [Guid("9E225954-7E37-4351-BA74-2FE1A45669A8")]
    public class ParameterChangedEventArgs<T> : EventArgs
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public ParameterChangedEventArgs(T old_value, T new_value)
        {
            this.OldValue = old_value;
            this.NewValue = new_value;
        }
    }
}
