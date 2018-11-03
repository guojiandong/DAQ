using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.PluginEngine
{
    [System.Runtime.InteropServices.Guid("EBDB7F12-2EBF-4BC3-86F4-566688829235")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public abstract class AbstractPluginBase : IPlugin
    {
        public IAppContext AppContext { get; private set; }

        public virtual string PlugInName
        {
            get { return this.GetType().FullName; }
        }

        public virtual void Initialize(IAppContext app)
        {
            this.AppContext = app;

            Console.WriteLine("AbstractPluginBase Initialize");
        }

        public virtual void Attach(IAppContext app)
        {
            Console.WriteLine("AbstractPluginBase Attach");
        }

        public virtual void Detach(IAppContext app)
        {
            Console.WriteLine("AbstractPluginBase Detach");
        }

        public virtual bool SupportUnload
        {
            get { return true; }
        }

        public virtual bool SupportLoad
        {
            get { return true; }
        }

        public virtual bool CheckCanLoad(bool isFirstCall)
        {
            return isFirstCall && this.SupportLoad;
        }



        public virtual string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
            }
        }

        public virtual Model.Settings.AbstractProfileSettings PlugInSettings
        {
            get
            {
                return null;
            }
        }
    }
}
