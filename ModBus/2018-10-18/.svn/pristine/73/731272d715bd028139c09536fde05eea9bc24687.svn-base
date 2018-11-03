using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    /// <summary>
    /// command line message.
    /// </summary>
    public class CommandLineMessage : AbstractMessageBase
    {

        #region Public Members
        /// <summary>
        /// get the current command name.
        /// </summary>
        public string CmdName { get; private set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string[] Parameters { get; private set; }
        #endregion

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(this.GetType().Name).Append(",").Append(SeqID).Append(",");
            str.Append(CmdName).Append(",");

            if(Parameters != null && Parameters.Length > 0)
                str.Append(String.Join(",", Parameters));

            return str.ToString();
        }

        #region Constructors
        /// <summary>
        /// new
        /// </summary>
        /// <param name="seqID"></param>
        /// <param name="cmdName"></param>
        /// <param name="parameters"></param>
        /// <exception cref="ArgumentNullException">cmdName is null</exception>
        public CommandLineMessage(int seqID, string cmdName, params string[] parameters) : base(seqID)
        {
            if (cmdName == null) throw new ArgumentNullException("cmdName");

            this.CmdName = cmdName;
            this.Parameters = parameters;
        }

        public void CopyFrom(CommandLineMessage other)
        {
            base.CopyFrom(other);
            this.CmdName = other.CmdName;
            this.Parameters = other.Parameters;
        }

        public override AbstractMessageBase Clone()
        {
            CommandLineMessage msg = new CommandLineMessage(this.SeqID, this.CmdName, this.Parameters);

            return msg;
        }

        #endregion


        #region Public Methods

        public override Packet ToPacket()
        {
            Base.Packet pkg = new Base.Packet(Encoding.UTF8.GetBytes(string.Concat(this.CmdName, " ", this.Parameters, Environment.NewLine)), SeqID);

            pkg.Tag = this.GetType().Name;
            return pkg;
        }

        //public 

        /// <summary>
        /// reply
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        public void Reply(Base.IConnection connection, string value)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            connection.BeginSend(ToPacket(value));
        }

        /// <summary>
        /// to <see cref="Base.Packet"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">value is null</exception>
        static public Base.Packet ToPacket(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return new Base.Packet(Encoding.UTF8.GetBytes(string.Concat(value, Environment.NewLine)));
        }
        #endregion
    }
}
