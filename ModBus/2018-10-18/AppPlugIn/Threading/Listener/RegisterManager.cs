using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Listener
{
    [Guid("5F2411E0-82DF-462B-A8A2-A2803C7B37E6")]
    public class RegisterManager<TListenerParam> : AbstractRegisterManager<TListenerParam, IListenerCallback<TListenerParam>>// where TListener : IListenerCallback<TListenerParam>
    {
        public RegisterManager()
        {
        }

        protected override void onDisposed()
        {
        }

        //public void RegisterListener(IListenerCallback<TListenerParam> listener)
        //{
        //    if (listener == null)
        //    {
        //        throw new ArgumentNullException("RegisterListener(), listener can't be null...");
        //    }

        //    doAddListener(listener);
        //}

        //public void UnregisterListener(IListenerCallback<TListenerParam> listener)
        //{
        //    if (listener == null)
        //    {
        //        throw new ArgumentNullException("UnregisterListener(), listener can't be null...");
        //    }

        //    doRemoveListener(listener);
        //}

        //public void NotifyAllListener(object sender, TListenerParam args)
        //{
        //    lock (mListenerList)
        //    {
        //        foreach(IListenerCallback<TListenerParam> listener in mListenerList)
        //        {
        //            NotifyListener(listener, sender, args);
        //        }
        //    }
        //}

        public override void NotifyListener(IListenerCallback<TListenerParam> listener, object sender, TListenerParam args)
        {
            //try
            //{
                if (typeof(ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
							&& ((ISynchronizeInvoke)listener).InvokeRequired)
                {
                    ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                    {
                        listener.onCallback(sender, args);
                    }), null);
                }
                else
                {
                    listener.onCallback(sender, args);
                }
            //}
            //catch(ObjectDisposedException){
                
            //}
            //catch (Exception)
            //{
            //}
        }

    }
}
