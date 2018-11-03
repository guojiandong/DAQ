using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn
{
    /// <summary>
    /// 组件信息注册接口
    /// 作用：应用程序将会第一时间从程序集找到实现了该接口的类并调用其CompoentRegister方法，从而被动的收集该组件的相关信息
    /// </summary>
    [Guid("B34B7FB2-25FB-464C-BB07-15180A87E2B1")]
    [ComVisible(true)]
    public interface IPlugInComponentConfig
    {
        IPlugInComponent ComponentRegister(IAppContext context, int idkey);
    }
}
