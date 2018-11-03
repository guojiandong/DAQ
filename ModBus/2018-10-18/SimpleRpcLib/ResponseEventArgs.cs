using System;
using System.Runtime.InteropServices;

namespace Ksat.SimpleRpcLib
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
        /// 发生异常
        /// </summary>
        public const int CAUSE_EXCEPTION = -101;

        /// <summary>
        /// 没有实现
        /// </summary>
        public const int NOT_IMPLEMENTED = -102;

        /// <summary>
        /// 参数错误
        /// </summary>
        public const int INVALID_PARAMETER = -103;

        //public const int SERVER_LOST = -104;

        /// <summary>
        /// 错误代码，0：成功，
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 返回的内容
        /// </summary>
        public string Value { get; set; }

        public ResponseEventArgs(int err, string value, int seqid = 0) : base(seqid)
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
