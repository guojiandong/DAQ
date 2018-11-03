using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Args
{
    #region base event args
    [Guid("F4388714-BC4A-49F1-9DD9-0520FC997783")]
    public class GenericEventArgs<T> : EventArgs
    {
        public readonly T Value;

        public GenericEventArgs(T v)
        {
            this.Value = v;
        }
    }
    #endregion

    #region inherit GenericEventArgs 
    [Guid("9B154789-BBD4-4813-A69C-78B7576279C2")]
    public class IntegerEventArgs : GenericEventArgs<int>
    {
        public IntegerEventArgs(int v) : base(v)
        {
        }
    }

    [Guid("AE1B4573-1941-40E9-BE52-CB789FD51471")]
    public class BoolEventArgs : GenericEventArgs<Boolean>
    {
        public BoolEventArgs(bool v) : base(v)
        {
        }
    }

    [Guid("E51EAAD3-0938-4356-8788-157F0FB99649")]
    public class StringEventArgs : GenericEventArgs<String>
    {
        public StringEventArgs(string v) : base(v)
        {
        }
    }

    [Guid("A9A8901E-649F-4D18-9F10-44F0FB4B668B")]
    public class ObjectEventArgs : GenericEventArgs<object>
    {
        public ObjectEventArgs(object v) : base(v)
        {
        }
    }
    #endregion
}
