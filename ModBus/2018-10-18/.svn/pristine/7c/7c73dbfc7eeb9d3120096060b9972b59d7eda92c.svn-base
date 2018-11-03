using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Com
{
    public class MultiSerialPortTask : ThreadTask, ISerialPortTaskManager
    {
        #region Members
        private LinkedList<SerialPortEx> mSerialPortList;
        private SemaphoreSlim mSemaphoreCounter;
        private ConcurrentQueue<ItemEventArgs> mReadySpList;
        #endregion

        public MultiSerialPortTask()
        {
            mSerialPortList = new LinkedList<SerialPortEx>();
            mReadySpList = new ConcurrentQueue<ItemEventArgs>();
            mSemaphoreCounter = new SemaphoreSlim(0, int.MaxValue);
        }

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
            System.Diagnostics.Trace.WriteLine(">>>>>>>>>>>>>>>>>>MultiSerialPortTask::doProc()>>>>>>>>>>>>>>>>>>>>>>");

            onTaskStarted();

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
                            //data.Sp.InvokeNotifyDataReceived(read_buffer, read_count);
                        }
                        else
                        {
                            // write....
                            data.Sp.doWrite(data.Buffer);
                        }
                        continue;
                    }
                    else
                    {
                        onProcIdle();
                    }

                    mSemaphoreCounter.Wait(getDefaultThreadTimeout());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("MultiSerialPortTask::doProc(), Exception:" + ex.ToString());
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

            onTaskStoped();

            System.Diagnostics.Trace.WriteLine("<<<<<<<<<<<<<<<<MultiSerialPortTask::doProc()<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        }

        protected virtual void onTaskStarted()
        {

        }

        protected virtual void onTaskStoped()
        {

        }

        protected virtual int getDefaultThreadTimeout()
        {
            return -1;
        }

        protected virtual void onProcIdle()
        {

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
            System.Diagnostics.Trace.WriteLine("MultiSerialPortTask::SerialPort_DataReceived(1)===PortName:" + sp.PortName);
#endif
            lock (mSerialPortList)
            {
                LinkedListNode<SerialPortEx> nodeNow = mSerialPortList.First;
                while (nodeNow != null)
                {
                    if (sp.PortName == nodeNow.Value.PortName)
                    {
#if DEBUG					
                        System.Diagnostics.Trace.WriteLine("MultiSerialPortTask::SerialPort_DataReceived(2)===PortName:" + sp.PortName);
#endif						
                        pushItem(new ItemEventArgs(nodeNow.Value));
                        return;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        public void Write(string tag, byte[] data, int offset, int count)
        {
            Write(GetSerialPortByTag(tag), data, offset, count);
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
        /// <param name="comPortName">serial port name</param>
        /// <param name="BaudRate"></param>
        /// <returns></returns>
        public SerialPortEx NewSerialPort(string comPortName, int BaudRate)
        {
            return NewSerialPort(comPortName, comPortName, BaudRate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag">tag for user custom</param>
        /// <param name="comPortName">serial port name</param>
        /// <param name="BaudRate"></param>
        /// <returns></returns>
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

            onSerialPortCreated(sp);

            sp.getSerialPort().DataReceived += SerialPort_DataReceived;

            lock (mSerialPortList)
                mSerialPortList.AddLast(sp);

            mSemaphoreCounter.Release();

            return sp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comPortName">serial port name</param>
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

            onSerialPortRemoved(sp);

            sp.Close();
        }

        public virtual void onSerialPortStatusChanged(SerialPortEx sp, bool isopened)
        {

        }

        protected virtual void onSerialPortCreated(SerialPortEx sp)
        {

        }

        protected virtual void onSerialPortRemoved(SerialPortEx sp)
        {

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
