using System;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    /// <summary>
    /// packet
    /// </summary>
    public class Packet
    {
        #region Members
        /// <summary>
        /// get or set sent size.
        /// </summary>
        internal int SentSize = 0;
        /// <summary>
        /// get the packet created time
        /// </summary>
        public readonly DateTime CreatedTime = Utils.DateHelper.UtcNow;
        /// <summary>
        /// get payload
        /// </summary>
        public readonly byte[] Payload;

        /// <summary>
        /// message id
        /// </summary>
        public int SeqID { get; }

        /// <summary>
        /// 
        /// </summary>
        public int SendCount { get; set; }

        public object UserData { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// new
        /// </summary>
        /// <param name="payload"></param>
        /// <exception cref="ArgumentNullException">payload is null.</exception>
        public Packet(byte[] payload, int seqid = 0)
        {
            if (payload == null) throw new ArgumentNullException("payload");
            this.Payload = payload;
            this.SeqID = seqid;
            this.SendCount = 0;
            this.SentSize = 0;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// get or set tag object
        /// </summary>
        public object Tag { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取一个值，该值指示当前packet是否已发送完毕.
        /// </summary>
        /// <returns>true表示已发送完毕</returns>
        public bool IsSent()
        {
            return this.SentSize >= this.Payload.Length;
        }
        #endregion
    }
}