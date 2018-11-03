using System;
using System.Runtime.InteropServices;

namespace Ksat.AppPlugIn.PluginEngine
{
    [System.Runtime.InteropServices.Guid("9E315EA7-0184-4790-AB8F-4039EA06ED5F")]
    [System.Runtime.InteropServices.ComVisible(true)]
    [Plugin("Gary", "E-mail:gary@jzspace.com", "测试组件", "(C)copyright 2015 ", "插件系统。")]
    public class DefaultPlugin : AbstractPluginBase
    {
        //public string PlugInName
        //{
        //    get { return this.GetType().FullName; }
        //}

        public override void Initialize(IAppContext app)
        {
            Console.WriteLine("DefaultPlugin Initialize");
        }

        public override void Attach(IAppContext app)
        {
            Console.WriteLine("DefaultPlugin Attach");
        }

        public override void Detach(IAppContext app)
        {
            Console.WriteLine("DefaultPlugin Detach");
        }

        public override bool SupportUnload
        {
            get { return true; }
        }

        public override bool SupportLoad
        {
            get { return true; }
        }

        public override bool CheckCanLoad(bool isFirstCall)
        {
            return true;
        }

        //public override string Version
        //{
        //    get
        //    {
        //        return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
        //    }
        //}
    }
}
