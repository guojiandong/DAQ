using System;
using System.Collections.Generic;

namespace Ksat.AppPlugIn.PluginEngine
{
    [System.Runtime.InteropServices.Guid("E81CC8AD-BB22-4EEF-B0AF-0C08EEC0C541")]
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class PluginInfo : AbstractPluginBaseInfo
    {
        /// <summary>
        /// 插件所在程序集, dll文件名称,不包含路径
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public string Assembly { get; internal set; }

        /// <summary>
        /// 构造器
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public System.Reflection.ConstructorInfo ConstructorInfo { get; set; }

        /// <summary>
        /// 实例对象
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public IPlugin PluginProvider { get; internal set; }

        /// <summary>
        /// 服务信息
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public PluginDescription PluginDescription { get; private set; }


        /// <summary>
        /// 服务状态
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public PluginState State { get; private set; }

        [System.Xml.Serialization.XmlIgnore]
        System.Reflection.ConstructorInfo constInfo = null;

        public bool IsAttached()
        {
            if(this.State == PluginState.Running)
            {
                return true;
            }

            return false;
        }


        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append(this.Assembly).Append(",");
            str.Append(this.TypeName).Append(",");
            str.Append(this.Enabled).Append(",");
            str.Append(",Depands:").Append(String.Join("、", Depands));

            return str.ToString();
        }
        

        /// <summary>
        /// 构造函数
        /// </summary>
        public PluginInfo() : base()
        {
            State = PluginState.NotInstalled;
        }

        public bool Initialize(IAppContext app)
        {
            if(app.Settings != null)
            {
                this.AutoAttach = app.Settings.IsPluginSupportAutoAttach(this);
            }

            return Load(app, false);
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <returns></returns>
        public bool Attach(IAppContext app)
        {
            return Load(app, true);
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        public bool Detach(IAppContext app)
        {
            if (PluginProvider == null || State == PluginState.Unload || !PluginProvider.SupportUnload)
            {
                return false;
            }
            else
            {
                PluginProvider.Detach(app);
                State = PluginState.Unload;

                return true;
            }
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="isInitializingCall">是否是初始化加载</param>
        /// <returns></returns>
        internal bool Load(IAppContext app, bool isInitializingCall)
        {
            if (EnsureLoadAssembly() && CreateProviderInstance() && InitialzingPluginProvider(app))
            {
                LoadService(app, isInitializingCall);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 加载程序集信息
        /// </summary>
        /// <returns></returns>
        private bool EnsureLoadAssembly()
        {
            if (constInfo != null) return true;

            System.Type tp = null;
            State = PluginState.NotInstalled;

            if (string.IsNullOrEmpty(Assembly))
            {
                try
                {
                    tp = System.Type.GetType(TypeName);
                }
                catch (Exception) { return false; }
            }
            else
            {
                string file = LocateAssemblyPath(Assembly);
                if (file == null) return false;
                try
                {
                    tp = System.Reflection.Assembly.LoadFile(file).GetType(TypeName);
                }
                catch (Exception) { return false; }
            }

            State = PluginState.LoadingError;
            if (tp == null) return false;

            //获得插件的信息
            object[] infos = tp.GetCustomAttributes(typeof(PluginAttribute), true);
            if (infos != null && infos.Length > 0)
            {
                this.PluginDescription = (infos[0] as PluginAttribute).Description;
            }
            else
            {
                this.PluginDescription = new PluginDescription("Gary", "Ksat", tp.Name, "", tp.FullName);
            }

            if ((constInfo = tp.GetConstructor(System.Type.EmptyTypes)) == null) return false;

            State = PluginState.TypeLoaded;
            return true;
        }

        /// <summary>
        /// 创建实例对象
        /// </summary>
        /// <returns></returns>
        private bool CreateProviderInstance()
        {
            if (PluginProvider != null) return true;

            if (State != PluginState.TypeLoaded) return false;
            else if (!Enabled) return false;
            else
            {
                try
                {
                    PluginProvider = constInfo.Invoke(new object[] { }) as IPlugin;


                    State = PluginState.UnInitialized;
                }
                catch (Exception)
                {
                    State = PluginState.LoadingError;
                    return false;
                }

                return PluginProvider != null;
            }
        }

        /// <summary>
        /// 初始化指定插件
        /// </summary>
        /// <returns></returns>
        private bool InitialzingPluginProvider(IAppContext app)
        {
            if (State != PluginState.UnInitialized)
            {
                return PluginProvider != null;
            }
            else
            {
                PluginProvider.Initialize(app);
                return true;
            }
        }

        /// <summary>
        /// 加载指定插件
        /// </summary>
        private bool LoadService(IAppContext app)
        {
            return LoadService(app, false);
        }

        /// <summary>
        /// 加载指定插件
        /// </summary>
        /// <param name="onloadCall">是否是初始化时候的请求</param>
        internal bool LoadService(IAppContext app, bool onloadCall)
        {
            if (PluginProvider == null || State == PluginState.Running || !PluginProvider.CheckCanLoad(onloadCall))
            {
                return false;
            }
            else
            {
                PluginProvider.Attach(app);
                State = PluginState.Running;

                return true;
            }
        }

        

        /// <summary>
        /// 确定程序集路径
        /// </summary>
        /// <param name="dllName">程序集名称</param>
        /// <returns>存在的路径.如果找不到,则返回null</returns>
        string LocateAssemblyPath(string dllName)
        {
            string RootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //查找同目录
            string path = System.IO.Path.Combine(RootPath, dllName);
            if (System.IO.File.Exists(path)) return path;

            path = System.IO.Path.Combine(RootPath, "plugins", dllName);
            if (System.IO.File.Exists(path)) return path;

            return null;
        }
    }
}
