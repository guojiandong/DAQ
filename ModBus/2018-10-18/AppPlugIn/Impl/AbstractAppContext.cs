using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Model;
using Ksat.AppPlugIn.Model.Settings;
using Ksat.AppPlugIn.PluginEngine;
using Ksat.AppPlugIn.Threading.Task;
using Ksat.AppPlugIn.UiCtrl;

namespace Ksat.AppPlugIn.Impl
{
    public abstract class AbstractAppContext : IAppContext, IDisposable
    {
        public AbstractAppContext()
        {
            this.mComponents = new PluginList();// LinkedList<PluginInfo>();
        }

        public abstract string AppName { get; protected set; }

        public abstract string AppVersion { get; protected set; }

        public abstract string AppBaseDir { get; protected set; }

        public UserInfo SessionUserInfo
        {
            get;
            set;
        }

        //public virtual UserInfo SessionUserInfo => throw new NotImplementedException();

        public abstract Dictionary<string, object> AppCache { get; protected set; }

        public abstract AbstractAppFormContainer AppFormContainer { get;}



        public abstract MainProfileSettings Settings { get; }

        public abstract Threading.ThreadTask MainTaskInstance { get; }

        public PluginList PlugIns
        {
            get
            {
                return mComponents;//.AsEnumerable();
            }
        }

        private PluginList mComponents;

        public PluginInfo Detach(string pluginName)
        {
            if (String.IsNullOrEmpty(pluginName))
            {
                throw new ArgumentNullException("pluginName can't be null...");
            }

            lock (mComponents)
            {
                LinkedListNode<PluginInfo> node = mComponents.First;
                while (node != null)
                {
                    if (node.Value.TypeName == pluginName)
                    {
                        node.Value.Detach(this);
                        return node.Value;
                    }
                    node = node.Next;
                }
            }

            return null;
        }

        public bool Attach(string pluginName)
        {
            if (String.IsNullOrEmpty(pluginName))
            {
                throw new ArgumentNullException("pluginName can't be null...");
            }

            lock (mComponents)
            {
                LinkedListNode<PluginInfo> node = mComponents.First;
                while (node != null)
                {
                    if (node.Value.TypeName == pluginName)
                    {
                        return Attach(node.Value);
                    }
                    node = node.Next;
                }
            }

            return false;
        }

        public bool Attach(PluginInfo comp)
        {
            if (comp == null)
            {
                throw new ArgumentNullException("PluginInfo can't be null...");
            }

            return comp.Attach(this);
        }

        public virtual void LoadPlugInComponents(string path = "")
        {
            // Setting_PlugIn_Dir
            //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Ksat.SmartFormPlatform.Properties.Settings.Default.Setting_PlugIn_Dir);
            //Type targetFormType = typeof(Form);

            //List<string> validpath = new List<string>();
            //foreach(string s in paths)
            if(!String.IsNullOrEmpty(path))
            {
                if (!System.IO.Directory.Exists(path))
                {
                    path = "";
                    Logging.Logger.Warn("LoadPlugInComponents(2), plugin path not exist:" + path);
                }
            }
            
            PluginList list = PluginHelper.GetPlugins(AppDomain.CurrentDomain.BaseDirectory, path);
            if (list != null)
            {
                SyncProfileSettings(list);

                list.Dump();

                // 调整依赖关系项目
                list.ReSort();

                mComponents = list;

                list.Dump();
            }
        }

        private void SyncProfileSettings(PluginList list)
        {
            LinkedListNode<PluginInfo> node = list.First;
            while (node != null)
            {
                AbstractPluginBaseInfo info = this.Settings.GetPlugin(node.Value);
                if (info != null)
                {
                    node.Value.Enabled = info.Enabled;
                    node.Value.AutoAttach = info.AutoAttach;
                }

                node = node.Next;
            }
        }

        public void StorePluginSettings()
        {
            LinkedListNode<PluginInfo> node = this.PlugIns.First;
            while (node != null)
            {
                AbstractPluginBaseInfo info = this.Settings.GetPlugin(node.Value);
                if (info == null)
                {
                    this.Settings.PlugInSettings.Add(new AbstractPluginBaseInfo(node.Value));
                }
                else
                {
                    info.Enabled = node.Value.Enabled;
                    info.AutoAttach = node.Value.AutoAttach;
                }

                node = node.Next;
            }
        }

        public void Dispose()
        {
            StorePluginSettings();
        }
    }
}
