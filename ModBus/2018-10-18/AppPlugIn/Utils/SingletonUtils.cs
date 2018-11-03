using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils
{
    //public abstract class SingletonUtils<T> where T : new() //new()，new不支持非公共的无参构造函数   
    public abstract class SingletonUtils<T> where T : class
    {
        private static T sInstance;

        public static T Instance()
        {
            lock (typeof(T))
            {
#if false
                if (sInstance == null)
                    sInstance = new T();
#else
                if (sInstance == null)
                    sInstance = Activator.CreateInstance<T>();
#endif
            }

            return sInstance;
        }

        protected SingletonUtils()
        {
            if (sInstance == null)
                sInstance = this as T;
        }
    }
}
