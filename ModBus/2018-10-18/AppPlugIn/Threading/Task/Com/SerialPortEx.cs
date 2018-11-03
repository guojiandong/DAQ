using Ksat.AppPlugIn.Communicate.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Com
{
    public class SerialPortEx : IDisposable
    {
        private readonly SerialPort mSerialPort;
        private readonly ISerialPortTaskManager mManager;
        private string mTag;
        private LinkedList<IDataReceivedListener> mListenerList;
        private volatile bool mIsDisposed;

        public SerialPortEx() : this(null, "COM1", "")
        {
            //throw new UnauthorizedAccessException("Can't call this conctructor...");
        }

        public SerialPortEx(string comPortName, int baudRate) : this(null, "COM1", "")
        {
            this.BaudRate = baudRate;
        }

        internal SerialPortEx(ISerialPortTaskManager task, string comPortName, string tag)
        {
            mIsDisposed = false;
            mSerialPort = new SerialPort(comPortName);
            mManager = task;
            mListenerList = new LinkedList<IDataReceivedListener>();
            mTag = (String.IsNullOrEmpty(tag) ? comPortName : tag);
            ReadTimeout = 100;
            DelayReadTime = 50;
            RepeatReadCount = 0;
            EnableDiscardInBuffer = true;
            EnableDiscardOutBuffer = true;
        }

        public string getTag()
        {
            return mTag;
        }

        internal SerialPort getSerialPort()
        {
            return mSerialPort;
        }

        public void SetDataReceivedEventHandler(SerialDataReceivedEventHandler listener)
        {
            mSerialPort.DataReceived += listener;
        }

        public void Dispose()
        {
            if (mIsDisposed)
            {
                System.Diagnostics.Trace.WriteLine("SerialPortEx::Dispose(), serial closed...");
                return;
            }

            mIsDisposed = true;
            //throw new NotImplementedException();

            mSerialPort.Close();
            if (mManager != null)
            {
                mManager.onSerialPortStatusChanged(this, false);
                mManager.DeleteSerialPort(PortName);
            }

        }

        public bool IsDisposed()
        {
            return mIsDisposed;
        }

        //public void Open(string comPortName)
        //{

        //}

        public void Open()
        {
            if (mIsDisposed)
            {
                throw new FieldAccessException("This SerialPortEx has disposed...");
            }

            if (mSerialPort.IsOpen)
            {
#if DEBUG
                System.Diagnostics.Trace.WriteLine("SerialPortEx::Open(), opened...");
#endif
                return;
            }

            mSerialPort.Open();
#if DEBUG
            System.Diagnostics.Trace.WriteLine("SerialPortEx::Open(), result:" + mSerialPort.IsOpen);
#endif
            if (mManager != null)
            {
                mManager.onSerialPortStatusChanged(this, mSerialPort.IsOpen);
            }
        }

        public void Close()
        {
            if (!mSerialPort.IsOpen)
            {
                System.Diagnostics.Trace.WriteLine("SerialPortEx::Close(), closed...");
                return;
            }

            mSerialPort.Close();

            if (mManager != null)
            {
                mManager.onSerialPortStatusChanged(this, mSerialPort.IsOpen);
            }
        }

        internal int doRead(byte[] buffer, int offset, int count)
        {
            if (IsDisposed() || !mSerialPort.IsOpen)
            {
                System.Diagnostics.Trace.WriteLine("doRead(), This SerialPortEx has disposed or closed...");
                return 0;
            }

            if (DelayReadTime > 0)
            {
                Thread.Sleep(DelayReadTime);
            }

            int length = 0, len = 0, repeatcount = 0;
            int total_data_len = mSerialPort.BytesToRead;

#if DEBUG
            System.Diagnostics.Trace.WriteLine("doRead(0), offset:" + offset
                + ", count:" + count + ", DelayReadTime:" + DelayReadTime
                + ", total_data_len:" + total_data_len);
#endif

            while (length < count && (length < total_data_len || repeatcount < RepeatReadCount))
            {
                try
                {
                    len = mSerialPort.Read(buffer, offset + length, count - length);
#if DEBUG
                    System.Diagnostics.Trace.WriteLine("doRead(1), total_data_len:" + total_data_len
                        + ", repeatcount:" + repeatcount
                        + ", length:" + length
                        + ", offset:" + offset
                        + ", len:" + len);
#endif
                    if (len <= 0)
                    {
                        repeatcount++;

                        if (!mSerialPort.IsOpen) break;
                    }
                    else
                    {
                        repeatcount = 0;
                        length += len;
                    }
                }
                catch (Exception ex)
                {
                    repeatcount++;
#if DEBUG
                    System.Diagnostics.Trace.WriteLine("doRead(4), total_data_len:" + total_data_len
                        + ", repeatcount:" + repeatcount
                        + ", length:" + length
                        + ", RepeatReadCount:" + RepeatReadCount
                        + ", ReadTimeout:" + this.ReadTimeout
                         + ", error:" + ex.ToString());
#endif
                    if (!mSerialPort.IsOpen) break;
                }

                //readTime = Convert.ToInt32(new TimeSpan(DateTime.Now.Ticks).Subtract(startTime).Duration().TotalMilliseconds);
            }
#if DEBUG
            System.Diagnostics.Trace.WriteLine("doRead(7), total_data_len:" + total_data_len
                + ", repeatcount:" + repeatcount
                + ", length:" + length
                + ", RepeatReadCount:" + RepeatReadCount
                + ", EnableDiscardInBuffer:" + EnableDiscardInBuffer);
#endif
            if (EnableDiscardInBuffer)
                mSerialPort.DiscardInBuffer();

            return length;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return doRead(buffer, offset, count);
        }

        internal void doWrite(byte[] buffer/*, int offset, int count*/)
        {
            doWrite(buffer, 0, buffer.Length);
        }

        internal void doWrite(byte[] buffer, int offset, int count)
        {
            if (IsDisposed() || !mSerialPort.IsOpen)
            {
                System.Diagnostics.Trace.WriteLine("doWrite(), This SerialPortEx has disposed or closed...");
                return;
            }

            if (buffer == null)
            {
                return;
            }

            if (EnableDiscardOutBuffer)
                mSerialPort.DiscardOutBuffer();
            mSerialPort.Write(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (IsDisposed())
            {
                System.Diagnostics.Trace.WriteLine("Write(), This SerialPortEx has disposed...");
                return;
            }
            if (mManager != null)
                mManager.Write(this, buffer, offset, count);
            else
                doWrite(buffer, offset, count);
        }

        public void Write(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                System.Diagnostics.Trace.WriteLine("Write(), text can't be empty...");
                return;
            }

            byte[] buff = Encoding.ASCII.GetBytes(text);
            Write(buff, 0, buff.Length);
        }

        public void WriteLine(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                System.Diagnostics.Trace.WriteLine("WriteLine(), text can't be empty...");
                return;
            }

            Write(String.Format("{0}{1}", text, mSerialPort.NewLine));
        }

        private void notifyListener(IDataReceivedListener listener, byte[] buffer, int count)
        {
            if (listener.GetType().IsSubclassOf(typeof(ISynchronizeInvoke))
                                && ((ISynchronizeInvoke)listener).InvokeRequired)
            {
                ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                {
                    listener.OnDataReceivedCallback(this, buffer, count);
                }), null);
            }
            else
            {
                listener.OnDataReceivedCallback(this, buffer, count);
            }
        }

        internal void InvokeNotifyDataReceived(byte[] buffer, int count)
        {
            if (buffer == null || count == 0)
            {
                System.Diagnostics.Trace.WriteLine("InvokeNotifyDataReceived(), count: " + count);
                return;
            }

            lock (mListenerList)
            {
                LinkedListNode<IDataReceivedListener> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    try
                    {
                        notifyListener(nodeNow.Value, buffer, count);
                    }
                    catch (ObjectDisposedException)
                    {
                        LinkedListNode<IDataReceivedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mListenerList.Remove(nodeDel);
                        continue;
                    }
                    catch (Exception)
                    {
                    }

                    nodeNow = nodeNow.Next;
                }

                //mListenerList.ForEach((c) => c.onDataReceivedCallback(this, buffer, count));
            }
        }

        internal void notifyDataReceived(byte[] buffer, int count)
        {
            if (buffer == null || count == 0)
            {
                System.Diagnostics.Trace.WriteLine("notifyDataReceived(), count: " + count);
                return;
            }

            //if (mManager.getMainThreadHandler() != null && mManager.getMainThreadHandler().InvokeRequired)
            //{
            //    byte[] data = new byte[count];
            //    Array.Copy(buffer, data, count);

            //    mManager.getMainThreadHandler().BeginInvoke(new EventHandler(delegate
            //    {
            //        lock (mListenerList)
            //        {
            //            LinkedListNode<IDataReceivedListener> nodeNow = mListenerList.First;
            //            while (nodeNow != null)
            //            {
            //                nodeNow.Value.onDataReceivedCallback(this, data, count);
            //                nodeNow = nodeNow.Next;
            //            }
            //                //mListenerList.ForEach((c) => c.onDataReceivedCallback(this, data, count));
            //        }

            //    }), null);
            //}
            //else
            {
                lock (mListenerList)
                {
                    LinkedListNode<Communicate.Base.IDataReceivedListener> nodeNow = mListenerList.First;
                    while (nodeNow != null)
                    {
                        nodeNow.Value.OnDataReceivedCallback(this, buffer, count);
                        nodeNow = nodeNow.Next;
                    }

                    //mListenerList.ForEach((c) => c.onDataReceivedCallback(this, buffer, count));
                }
            }
        }

        public void RegisterDataListener(Communicate.Base.IDataReceivedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("registerDataListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                if (mListenerList.Contains(listener)) return;
                mListenerList.AddLast(listener);
            }
        }

        public void UnregisterDataListener(IDataReceivedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("unregisterDataListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                mListenerList.Remove(listener);
            }
        }


        #region Members
        public int DelayReadTime { get; set; }

        public int RepeatReadCount { get; set; }

        public string Tag { get { return mTag; } set { mTag = value; } }

        [DefaultValue(true)]
        public bool EnableDiscardInBuffer { get; set; }

        [DefaultValue(true)]
        public bool EnableDiscardOutBuffer { get; set; }

        [Browsable(true)]
        [DefaultValue(Handshake.None)]
        [MonitoringDescription("Handshake")]
        public Handshake Handshake { get { return mSerialPort.Handshake; } set { mSerialPort.Handshake = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("Encoding")]
        public Encoding Encoding { get { return mSerialPort.Encoding; } set { mSerialPort.Encoding = value; } }

        [Browsable(true)]
        [DefaultValue(true)]
        [MonitoringDescription("DtrEnable")]
        public bool DtrEnable { get { return mSerialPort.DtrEnable; } set { mSerialPort.DtrEnable = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CtsHolding { get { return mSerialPort.CtsHolding; } }

        [Browsable(true)]
        [DefaultValue(false)]
        [MonitoringDescription("DiscardNull")]
        public bool DiscardNull { get { return mSerialPort.DiscardNull; } set { mSerialPort.DiscardNull = value; } }

        [Browsable(true)]
        [DefaultValue(8)]
        [MonitoringDescription("DataBits")]
        public int DataBits { get { return mSerialPort.DataBits; } set { mSerialPort.DataBits = value; } }

        [Browsable(false)]
        public bool IsOpen { get { return mSerialPort.IsOpen; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DsrHolding { get { return mSerialPort.DsrHolding; } }

        [Browsable(false)]
        [DefaultValue("\n")]
        [MonitoringDescription("NewLine")]
        public string NewLine { get { return mSerialPort.NewLine; } set { mSerialPort.NewLine = value; } }

        [Browsable(true)]
        [DefaultValue(4096)]
        [MonitoringDescription("ReadBufferSize")]
        public int ReadBufferSize { get { return mSerialPort.ReadBufferSize; } set { mSerialPort.ReadBufferSize = value; } }

        [Browsable(true)]
        [DefaultValue(63)]
        [MonitoringDescription("ParityReplace")]
        public byte ParityReplace { get { return mSerialPort.ParityReplace; } set { mSerialPort.ParityReplace = value; } }

        [Browsable(true)]
        [DefaultValue("COM1")]
        [MonitoringDescription("PortName")]
        public string PortName { get { return mSerialPort.PortName; } set { mSerialPort.PortName = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CDHolding { get { return mSerialPort.CDHolding; } }

        [Browsable(true)]
        [DefaultValue(-1)]
        [MonitoringDescription("ReadTimeout")]
        public int ReadTimeout { get { return mSerialPort.ReadTimeout; } set { mSerialPort.ReadTimeout = value; } }

        [Browsable(true)]
        [DefaultValue(1)]
        [MonitoringDescription("ReceivedBytesThreshold")]
        public int ReceivedBytesThreshold { get { return mSerialPort.ReceivedBytesThreshold; } set { mSerialPort.ReceivedBytesThreshold = value; } }

        [Browsable(true)]
        [DefaultValue(false)]
        [MonitoringDescription("RtsEnable")]
        public bool RtsEnable { get { return mSerialPort.RtsEnable; } set { mSerialPort.RtsEnable = value; } }

        [Browsable(true)]
        [DefaultValue(StopBits.One)]
        [MonitoringDescription("StopBits")]
        public StopBits StopBits { get { return mSerialPort.StopBits; } set { mSerialPort.StopBits = value; } }

        [Browsable(true)]
        [DefaultValue(2048)]
        [MonitoringDescription("WriteBufferSize")]
        public int WriteBufferSize { get { return mSerialPort.WriteBufferSize; } set { mSerialPort.WriteBufferSize = value; } }

        [Browsable(true)]
        [DefaultValue(-1)]
        [MonitoringDescription("WriteTimeout")]
        public int WriteTimeout { get { return mSerialPort.WriteTimeout; } set { mSerialPort.WriteTimeout = value; } }

        [Browsable(true)]
        [DefaultValue(Parity.None)]
        [MonitoringDescription("Parity")]
        public Parity Parity { get { return mSerialPort.Parity; } set { mSerialPort.Parity = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BytesToRead { get { return mSerialPort.BytesToRead; } }

        [Browsable(true)]
        [DefaultValue(9600)]
        [MonitoringDescription("BaudRate")]
        public int BaudRate { get { return mSerialPort.BaudRate; } set { mSerialPort.BaudRate = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BreakState { get { return mSerialPort.BreakState; } set { mSerialPort.BreakState = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Stream BaseStream { get { return mSerialPort.BaseStream; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BytesToWrite { get { return mSerialPort.BytesToWrite; } }

        #endregion
    }
}
