using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils.Serializer
{
    public class MemoryBinarySerializer
    {
        /// <summary>
        /// 对象深克隆
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object DeepClone(object obj)
        {
            //object obj = null;
            //将对象序列化成内存中的二进制流  
            BinaryFormatter inputFormatter = new BinaryFormatter();
            MemoryStream inputStream;
            using (inputStream = new MemoryStream())
            {
                inputFormatter.Serialize(inputStream, obj);
            }

            object objclone = null;
            //将二进制流反序列化为对象  
            using (MemoryStream outputStream = new MemoryStream(inputStream.ToArray()))
            {
                BinaryFormatter outputFormatter = new BinaryFormatter();
                objclone = outputFormatter.Deserialize(outputStream);
            }

            return objclone;
        }
        
    }
}
