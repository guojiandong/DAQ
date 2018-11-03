using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.Base
{
    public interface IDataReceivedListener
    {
        void OnDataReceivedCallback(object sender, byte[] buffer, int count);
    }
}
