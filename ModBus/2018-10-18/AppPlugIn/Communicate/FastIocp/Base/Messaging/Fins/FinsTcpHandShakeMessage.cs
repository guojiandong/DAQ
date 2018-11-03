using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging.Fins
{
    /// <summary>
    /// 握手消息
    /// </summary>
    public class FinsTcpHandShakeMessage : AbstractFinsTcpMessage
    {
        /// <summary>
        /// 本地ip地址
        /// </summary>
        public UInt32 LocalIpAddress { get; set; }

        /// <summary>
        /// 远程ip地址
        /// </summary>
        public UInt32 RemoteIpAddress { get; set; }


        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("FinsHandShake").Append(",");
            str.Append(this.Command).Append(",");
            str.AppendFormat("{0:X8}", this.ErrorCode).Append(",");
            str.AppendFormat("{0:X8}", this.LocalIpAddress).Append(",");
            str.AppendFormat("{0:X8}", this.RemoteIpAddress);
            return str.ToString();
        }

        public bool IsResponse()
        {
            if (this.Command == CommandCode.HandShake_Resp)
            {
                return true;
            }

            return false;
        }

        public override AbstractMessageBase Clone()
        {
            //throw new NotImplementedException();
            return new FinsTcpHandShakeMessage(this);
        }

        public void CopyFrom(FinsTcpHandShakeMessage other)
        {
            base.CopyFrom(other);

            this.LocalIpAddress = other.LocalIpAddress;
            this.RemoteIpAddress = other.RemoteIpAddress;
        }

        public FinsTcpHandShakeMessage(FinsTcpHandShakeMessage other) : base(other.Command, other.SeqID)
        {
            CopyFrom(other);
        }

        public FinsTcpHandShakeMessage(CommandCode cmd, byte[] buffer, int offset, int count) : base(cmd, 0)
        {
            this.LocalIpAddress = (UInt32)Utils.NetworkBitConverter.ToInt32(buffer, offset);
            offset += 4;

            if (IsResponse())
            {
                this.RemoteIpAddress = (UInt32)Utils.NetworkBitConverter.ToInt32(buffer, offset);
                offset += 4;
            }
            else
            {
                this.RemoteIpAddress = 0;
            }
        }

        public FinsTcpHandShakeMessage(string ipaddress, int seqID = 0) : base(CommandCode.HandShake_Req, seqID)
        {
            if (String.IsNullOrEmpty(ipaddress)) throw new ArgumentNullException("ipaddress can't be null.");

            //System.Net.IPAddress ip = Utils.NetUtils.GetIpAddress(ipaddress);
            //if(ip == null)
            //{
            //    throw new ArgumentException("parse ip address:"+ipaddress+" failed.");
            //}

            this.LocalIpAddress = (UInt32)System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(ipaddress));

            this.RemoteIpAddress = 0;
        }

        public FinsTcpHandShakeMessage(string ipaddress, string remoteip, int seqID = 0) : this(ipaddress, seqID)
        {
            if (String.IsNullOrEmpty(remoteip)) throw new ArgumentNullException("remoteip can't be null.");

            //System.Net.IPAddress ip = Utils.NetUtils.GetIpAddress(remoteip);
            //if (ip == null)
            //{
            //    throw new ArgumentException("parse remote ip address:" + remoteip + " failed.");
            //}

            this.RemoteIpAddress = (UInt32)System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(remoteip));

            this.Command = CommandCode.HandShake_Resp;
        }

        public FinsTcpHandShakeMessage(IPEndPoint ipaddress, int seqID = 0) : base(CommandCode.HandShake_Req, seqID)
        {
            if (ipaddress == null) throw new ArgumentNullException("remoteip can't be null.");

            this.LocalIpAddress = (UInt32)System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(ipaddress.Address));
        }

        public FinsTcpHandShakeMessage(IPEndPoint ipaddress, IPEndPoint remoteip, int seqID = 0) : this(ipaddress, seqID)
        {
            if (remoteip == null) throw new ArgumentNullException("remoteip can't be null.");

            this.RemoteIpAddress = (UInt32)System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(remoteip.Address));

            this.Command = CommandCode.HandShake_Resp;
        }

        public override Packet ToPacket()
        {
            int datalength = 12;
            if(Command == CommandCode.HandShake_Resp)
            {
                datalength += 4;
            }

            byte[] payload = new byte[datalength + 8];

            int offset = FixHeader(payload, datalength);

            ///// fill data length
            //Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(datalength), 0, payload, offset, 4);
            //offset += 4;

            ///// fill command
            //Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes((int)this.Command), 0, payload, offset, 4);
            //offset += 4;

            ///// fill error code
            //Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(this.ErrorCode), 0, payload, offset, 4);
            //offset += 4;
#if true
            payload[offset + 3 ] = Utils.NetworkBitConverter.GetBytes(this.LocalIpAddress)[0];
#else
            /// fill local ip address
            Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(this.LocalIpAddress), 0, payload, offset, 4);
#endif
            offset += 4;

            /// fill remote ip address
            if (Command == CommandCode.HandShake_Resp)
            {
                Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(this.RemoteIpAddress), 0, payload, offset, 4);
                offset += 4;
            }

            Base.Packet pkg = new Base.Packet(payload, this.SeqID);
            pkg.Tag = this.GetType().Name;

            return pkg;
        }
    }
}
