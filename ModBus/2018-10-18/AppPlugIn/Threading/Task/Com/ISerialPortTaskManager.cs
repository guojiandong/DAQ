using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Com
{
    public interface ISerialPortTaskManager
    {
        void DeleteSerialPort(string comPortName);
        void Write(SerialPortEx sp, byte[] data, int offset, int count);

        void onSerialPortStatusChanged(SerialPortEx sp, bool isopened);
    }
}
