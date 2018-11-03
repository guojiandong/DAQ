using System;
using System.Runtime.InteropServices;

namespace KsatRpcLibrary
{
    [Guid("A4C18AF0-6467-4BA4-97DE-9C85DBEE3705")]
    [Serializable]
    public class RequestActionEventArgs : AbstractCommunicateEventargs
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 传输内容
        /// </summary>
        public string Value { get; set; }

        public RequestActionEventArgs(string act, string value) : base(new Random().Next(10, int.MaxValue))
        {
            this.Action = act;
            this.Value = value;
        }

        public ResponseEventArgs ToResponse(string value, int err = ResponseEventArgs.SUCCESS)
        {
            return new ResponseEventArgs(value, err, this.SeqID);
        }

        public override string ToString()
        {
            return String.Format("{0}, SeqID:{1}, Action:{2}, Value:{3}", this.GetType().Name,
                this.SeqID, this.Action, this.Value);
        }
    }

}
