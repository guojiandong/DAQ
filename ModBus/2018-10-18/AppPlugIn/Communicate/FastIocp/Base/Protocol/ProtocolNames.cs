
namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    /// <summary>
    /// ProtocolNames
    /// </summary>
    static public class ProtocolNames
    {
        /// <summary>
        /// 原始字节流
        /// </summary>
        public const string Default = "binary";

        /// <summary>
        /// thrift协议
        /// </summary>
        public const string Thrift = "thrift";
        /// <summary>
        /// 命令行协议
        /// </summary>
        public const string CommandLine = "commandLine";

        /// <summary>
        /// 原始的字节码
        /// </summary>
        public const string OriginalBinary = "binary";

        /// <summary>
        /// 原始的字符串
        /// </summary>
        public const string OriginalString = "string";

        /// <summary>
        /// 欧姆龙
        /// </summary>
        public const string Fins_Tcp = "FINS/TCP";
    }


    public sealed class ProtocolManager : Utils.SingletonUtils<ProtocolManager>
    {
        public enum ProtocolName : int
        {
            Default = 0,

            Binary,

            String,

            Thrift,

            CommandLine,

            FinsTcp,

            MaxCount
        }

        public ProtocolManager()
        {
        }

    }
}