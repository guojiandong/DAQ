using Ksat.AppPlugIn.Model.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Cmd
{
    [Guid("3FA5A513-DF9F-4E4E-9ED6-BFF0ECC23FD6")]
    public class AbstractCommandEventArgs : ObjectEventArgs
    {
        public readonly bool IsRequest;

        public AbstractCommandEventArgs(bool isReq, object v) : base(v)
        {
            this.IsRequest = isReq;
        }
    }



    //public class AbstractIntCommandEventArgs : CommandEventArgs<int>
    //{
    //    public readonly bool IsRequest;

    //    public AbstractIntCommandEventArgs(int cmd, object v, bool isReq) : base(cmd, v)
    //    {
    //        this.IsRequest = isReq;
    //    }
    //}

    //public class AbstractStringCommandEventArgs : CommandEventArgs<string>
    //{
    //    public readonly bool IsRequest;

    //    public AbstractStringCommandEventArgs(string cmd, object v, bool isReq) : base(cmd, v)
    //    {
    //        this.IsRequest = isReq;
    //    }
    //}
}
