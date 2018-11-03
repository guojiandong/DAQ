using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils.Serializer
{
    internal class BinarySerializer : AbstractSerializer
    {
        public override void Serializer(object obj, string path)
        {
            if (obj == null)
            {
                throw new ArgumentException("Invalid parameters...");
            }

            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, obj);

            stream.Flush();


            stream.ToArray();


            //XmlSerializer ser = new XmlSerializer(obj.GetType());
            //StreamWriter stream = new StreamWriter(path);
            //ser.Serialize(stream, obj);
            //stream.Flush();
            //stream.Close();
        }

        public override T Deserialize<T>(string path)
        {
            throw new NotImplementedException();
        }

        //public T Deserialize<T>(string path)
        //{
        //    if (String.IsNullOrEmpty(path))
        //    {
        //        throw new ArgumentException("Invalid parameters...");
        //    }

        //    XmlSerializer ser = new XmlSerializer(typeof(T));
        //    StreamReader stream = new StreamReader(path);
        //    T result = (T)ser.Deserialize(stream);
        //    stream.Close();

        //    return result;
        //}
    }
}
