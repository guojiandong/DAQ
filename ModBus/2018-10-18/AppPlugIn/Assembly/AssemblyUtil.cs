using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Assembly
{
    public class AssemblyUtil
    {
        public static string GetAssemblyVersion(string path)
        {
            if (File.Exists(path))
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(path);
                return asm.GetName().Version.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
