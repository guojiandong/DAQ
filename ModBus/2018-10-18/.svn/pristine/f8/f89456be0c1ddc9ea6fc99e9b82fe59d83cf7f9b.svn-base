using System;
using System.Runtime.InteropServices;

namespace Ksat.SimpleRpcLib
{
	[Guid("F46079E9-8880-4F62-979D-F30DBCD40846")]
    [Serializable]
    public class RequestEventArgs : AbstractCommunicateEventargs
    {
        /// <summary>
        /// 工站ID， 从1开始
        /// </summary>
        public int MachineID { get; set; }

        /// <summary>
        /// 工位ID， 从1开始
        /// </summary>
        public int StationID { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 传输内容
        /// </summary>
        public string Value { get; set; }

        public RequestEventArgs(string value = "") : this(0, 0, value)
        {
            this.Value = value;
        }

        public RequestEventArgs(int machineid, int stationid, string value = "") : base(new Random().Next(10, int.MaxValue))
        {
            this.MachineID = machineid;
            this.StationID = stationid;
            this.Value = value;
        }

        public ResponseEventArgs ToResponse(int err, string value = "")
        {
            return new ResponseEventArgs(err, value, this.SeqID);
        }

        public override string ToString()
        {
            return String.Format("{0}:{1},{2}, Action:{3}, SeqID:{4}, Value:{5}", this.GetType().Name, this.MachineID, this.StationID,
                this.Action, this.SeqID, this.Value);
        }
    }
	
	
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

        public ResponseEventArgs ToResponse(int err, string value = "")
        {
            return new ResponseEventArgs(err, value, this.SeqID);
        }

        public override string ToString()
        {
            return String.Format("{0}, Action:{1}, SeqID:{2}, Value:{3}", this.GetType().Name,
                this.Action, this.SeqID, this.Value);
        }
    }

}
