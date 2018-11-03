using System;
using System.Runtime.InteropServices;

namespace KsatRpcLibrary
{
    [Guid("76183C8C-48A8-459B-8020-FE2E3E6C3ED2")]
    [Serializable]
    public class ResponseEventArgs : AbstractCommunicateEventargs
    {
        public const int SUCCESS = 0;

        /// <summary>
        /// 联机失败
        /// </summary>
        public const int NOT_CONNECT_TO_SERVER = -100;

        /// <summary>
        /// 拒接连接
        /// </summary>
        public const int CONNECT_REFUSED = -101;

        /// <summary>
        /// 拒接连接
        /// </summary>
        public const int CONNECT_LOSTED = -102;

        /// <summary>
        /// IPC初始化失败
        /// </summary>
        public const int IPC_INIT_FAILED = -103;

        /// <summary>
        /// 发生异常
        /// </summary>
        public const int CAUSE_EXCEPTION = -104;

        /// <summary>
        /// 没有实现
        /// </summary>
        public const int NOT_IMPLEMENTED = -105;

        /// <summary>
        /// 参数错误
        /// </summary>
        public const int INVALID_PARAMETER = -106;


        //public const int 

        public const int RESPONSE_TIMEOUT = -107;

        /// <summary>
        /// 错误代码，0：成功，
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 返回的内容
        /// </summary>
        public string Value { get; set; }

        public ResponseEventArgs(string value, int err = SUCCESS, int seqid = 0) : base(seqid)
        {
            this.ErrorCode = err;
            this.Value = value;
        }

        public bool IsSuccess()
        {
            if (this.ErrorCode == 0)
                return true;

            return false;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1},SeqID:{2}, Value:{3}", this.GetType().Name, this.ErrorCode,this.SeqID, this.Value);
        }
    }
}
