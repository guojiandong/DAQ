using Ksat.AppPlugIn.Common.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Base
{
    public enum SessionStatus : int
    {
        None = 0,
        
        Connected,

        Disconnected,

        Opened,

        Closed,

        //Starting, 


        //Started,


        //Stoping,


        //Stoped,


        Disposed,

        /// <summary>
        /// value Exception
        /// </summary>
        CauseError,
    }

    public interface ISessionStatusListener
    {
        void OnSessionStatusChanged(object sender, SessionStatus status, object value);
    }
}
