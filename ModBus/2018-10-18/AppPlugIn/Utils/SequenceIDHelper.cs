using System;

namespace Ksat.AppPlugIn.Utils
{
    public class SequenceIDHelper
    {
        private static SequenceIDHelper sInstance;

        public static SequenceIDHelper Instance()
        {
            lock (typeof(SequenceIDHelper))
            {
                if (sInstance == null)
                    sInstance = new SequenceIDHelper();
            }

            return sInstance;
        }

        private uint mSeqID;
        private ushort mShortSeqID;
        private object lockObject = new object();

        private SequenceIDHelper()
        {
            mSeqID = (uint)(new Random().Next(100, 1000));

            mShortSeqID = (ushort)(new Random().Next(10, 200));
        }

        public uint NextInt()
        {
            lock (lockObject)
            {
                return mSeqID++;
            }
        }

        public ushort NextShort()
        {
            lock (lockObject)
            {
                return mShortSeqID++;
            }
        }
    }
}
