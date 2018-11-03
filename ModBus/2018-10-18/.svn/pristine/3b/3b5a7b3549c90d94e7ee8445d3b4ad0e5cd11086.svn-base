using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Cmd
{
    public interface IResponseCommandEventListener<TCommand>
    {
        void onResponseCallback(object sender, TCommand command, object args);
    }


    [Guid("9DDCED44-526F-48B9-A090-E2BDE9358681")]
    public interface IResponseStringCommandEventListener : IResponseCommandEventListener<string>
    {
    }

    public interface IResponseIntCommandEventListener : IResponseCommandEventListener<int>
    {
    }

    public interface IResponseObjectCommandEventListener : IResponseCommandEventListener<object>
    {
    }

    //public interface IResponseCommandWithFilterEventListener
    //{
    //    void onResponseCallback(object sender, int command, int filter, object args);
    //}

}
