using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.V1
{
    public interface IConnection
    {
        string GetTag();
        void SetTag(string tag);
        void stop();
        int send(string data);
        int send(byte[] data, int offset, int count);
        int send(byte[] data, int offset, int size, System.Net.Sockets.SocketFlags socketFlags);
        //string sendSync(string data, int timeout);

        System.Net.Sockets.Socket GetSocket();

        string GetRemoteAddress();

        void registerDataListener(Communicate.Base.IDataReceivedListener listener);
        void unregisterDataListener(Communicate.Base.IDataReceivedListener listener);

        void registerStatusListener(ISocketStatusChangedListener listener);
        void unregisterStatusListener(ISocketStatusChangedListener listener);
    }

    public interface ISocketClient : IConnection
    {
        bool IsConnected();
    }

    
}
