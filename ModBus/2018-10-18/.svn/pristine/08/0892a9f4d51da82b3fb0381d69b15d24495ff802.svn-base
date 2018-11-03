using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Com
{
    public class SerialPortTask : ThreadTask, ISerialPortTaskManager
    {
        #region Members
        private LinkedList<SerialPortEx> mSerialPortList;
        private SemaphoreSlim mSemaphoreCounter;

        private ConcurrentQueue<ItemEventArgs> mReadySpList;


        #endregion

        #region Constructor
        public SerialPortTask(/*ISynchronizeInvoke control = null*/) : base()
        {
            mSerialPortList = new LinkedList<SerialPortEx>();
            mReadySpList = new ConcurrentQueue<ItemEventArgs>();
            mSemaphoreCounter = new SemaphoreSlim(0, int.MaxValue);
        }
        #endregion

        protected override void onCanceled()
        {
            mSemaphoreCounter.Release();

            base.onCanceled();

            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    mSemaphoreCounter.Release();
                    nodeNow.Value.Close();

                    nodeNow = nodeNow.Next;
                }
            }

            mSemaphoreCounter.Release();
        }

        protected override void doProc(object param)
        {
            int read_count = 0;
            byte[] read_buffer = new byte[1024];
            System.Diagnostics.Trace.WriteLine(">>>>>>>>>>>>>>>>>>SerialPortTask::doInBackground()>>>>>>>>>>>>>>>>>>>>>>");
            while (!IsCanceled())
            {
                try
                {
                    ItemEventArgs data;
                    if (mReadySpList.TryDequeue(out data))
                    {
                        if (data.Buffer == null)
                        {
                            // read....
                            read_count = data.Sp.doRead(read_buffer, 0, read_buffer.Length);

                            onDataReceived(data.Sp, read_buffer, read_count);

                            //if (getMainThreadHandler() != null && getMainThreadHandler().InvokeRequired)
                            //{
                            //    byte[] data_buff = new byte[read_count];
                            //    Array.Copy(read_buffer, data_buff, read_count);

                            //    getMainThreadHandler().BeginInvoke(new EventHandler(delegate
                            //    {
                            //        data.Sp.notifyDataReceived(data_buff, read_count);
                            //    }), null);
                            //}
                            //else
                            //{
                            //    data.Sp.notifyDataReceived(read_buffer, read_count);
                            //}
                        }
                        else
                        {
                            // write....
                            data.Sp.doWrite(data.Buffer);
                        }
                        continue;
                    }

                    mSemaphoreCounter.Wait();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("SerialPortTask::doInBackground(), Exception:" + ex.ToString());
                }
            }

            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    nodeNow.Value.Close();
                    nodeNow = nodeNow.Next;
                }

                mSerialPortList.Clear();
            }

            System.Diagnostics.Trace.WriteLine("<<<<<<<<<<<<<<<<SerialPortTask::doInBackground()<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        }

        protected virtual void onDataReceived(SerialPortEx sender, byte[] buffer, int count)
        {
#if DEBUG
            System.Diagnostics.Trace.WriteLine("MultiSerialPortTask::onDataReceived(4), count:" + count
                + ", portname:" + sender.PortName
                + ", tag:" + sender.Tag);
#endif
            sender.InvokeNotifyDataReceived(buffer, count);
        }

        private void pushItem(ItemEventArgs item)
        {
            mReadySpList.Enqueue(item);
            mSemaphoreCounter.Release();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
#if DEBUG
            System.Diagnostics.Trace.WriteLine("SerialPortTask::SerialPort_DataReceived(1)===PortName:" + sp.PortName);
#endif
            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    if (sp.PortName == nodeNow.Value.PortName)
                    {
#if DEBUG					
                        System.Diagnostics.Trace.WriteLine("SerialPortTask::SerialPort_DataReceived(2)===PortName:" + sp.PortName);
#endif						
                        pushItem(new ItemEventArgs(nodeNow.Value));
                        return;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        public void Write(SerialPortEx sp, byte[] data, int offset, int count)
        {
            if (sp == null)
            {
                throw new ArgumentNullException("SerialPortEx is null...");
            }

            byte[] buff = new byte[count];

            if (offset > 0)
            {
                for (int i = 0; i < count; i++)
                    buff[i] = data[i + offset];
            }
            else
            {
                Array.Copy(data, buff, count);
            }

            pushItem(new ItemEventArgs(sp, buff));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comPortName"></param>
        /// <param name="BaudRate"></param>
        /// <returns></returns>
        public SerialPortEx NewSerialPort(string comPortName, int BaudRate)
        {
            return NewSerialPort(comPortName, comPortName, BaudRate);
        }

        public SerialPortEx NewSerialPort(string tag, string comPortName, int BaudRate)
        {
            if (String.IsNullOrEmpty(comPortName))
            {
                throw new ArgumentNullException("SerialPort port name can't be empty...");
            }

            bool isValid = false;
            foreach (string p in SerialPort.GetPortNames())
            {
                if (p == comPortName)
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid)
            {
                throw new ArgumentNullException("SerialPort port name invalid...");
            }

            if (String.IsNullOrEmpty(tag))
                tag = comPortName;

            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    if (tag == nodeNow.Value.getTag())
                    {
                        return nodeNow.Value;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            SerialPortEx sp = new SerialPortEx(this, comPortName, tag);
            if (BaudRate > 0)
                sp.BaudRate = BaudRate;

            sp.getSerialPort().DataReceived += SerialPort_DataReceived;

            lock (mSerialPortList)
                mSerialPortList.AddLast(sp);

            mSemaphoreCounter.Release();

            return sp;
        }

        public virtual void onSerialPortStatusChanged(SerialPortEx sp, bool isopened)
        {

        }

        public void DeleteSerialPort(string comPortName)
        {
            if (String.IsNullOrEmpty(comPortName))
            {
                throw new ArgumentNullException("SerialPort port name can't be empty...");
            }

            SerialPortEx sp = null;
            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    if (comPortName == nodeNow.Value.PortName)
                    {
                        sp = nodeNow.Value;
                        break;
                    }
                    nodeNow = nodeNow.Next;
                }
            }

            DeleteSerialPort(sp);
        }

        public void DeleteSerialPort(SerialPortEx sp)
        {
            if (sp == null)
            {
                return;
            }

            lock (mSerialPortList)
                mSerialPortList.Remove(sp);

            sp.Close();
        }

        public SerialPortEx GetSerialPortByTag(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("SerialPort port name can't be empty...");
            }

            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    if (tag == nodeNow.Value.getTag())
                    {
                        return nodeNow.Value;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return null;
        }

        public SerialPortEx GetSerialPort(string comPortName)
        {
            if (String.IsNullOrEmpty(comPortName))
            {
                throw new ArgumentNullException("SerialPort port name can't be empty...");
            }

            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    if (comPortName == nodeNow.Value.PortName)
                    {
                        return nodeNow.Value;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return null;
        }

        public List<SerialPortEx> GetSerialPort()
        {
            lock (mSerialPortList)
            {
                return new List<SerialPortEx>(mSerialPortList);
            }
        }


        internal class ItemEventArgs
        {
            public readonly byte[] Buffer;
            public readonly SerialPortEx Sp;
            //public readonly bool IsRead;

            public ItemEventArgs(SerialPortEx sp) : this(sp, null)
            {

            }

            public ItemEventArgs(SerialPortEx sp, byte[] buff)
            {
                this.Sp = sp;
                this.Buffer = buff;
                //this.IsRead = read;
            }
        }
    }
}
