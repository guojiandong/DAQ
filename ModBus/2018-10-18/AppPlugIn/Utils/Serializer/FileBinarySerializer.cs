using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils.Serializer
{
    internal class FileBinarySerializer : AbstractSerializer
    {
        public override T Deserialize<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                T ret = (T)bf.Deserialize(fs);

                fs.Close();

                return ret;
            }
            catch (Exception ex)
            {
                fs.Close();
                throw ex;
            }
        }

        public override void Serializer(object obj, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            //if(fs == null)
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
            catch (Exception ex)
            {
                fs.Close();
                throw ex;
            }
        }
    }
}
