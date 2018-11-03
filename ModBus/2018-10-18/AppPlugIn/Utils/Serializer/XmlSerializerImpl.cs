using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Utils.Serializer
{
    internal class XmlSerializerImpl : AbstractSerializer
    {
        public override void Serializer(object obj, string path)
        {
            if (obj == null || String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid parameters...");
            }

            XmlSerializer ser = new XmlSerializer(obj.GetType());
            StreamWriter stream = new StreamWriter(path);

            try
            {
                ser.Serialize(stream, obj);
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                stream.Close();

                throw ex;
            }
        }

        public override T Deserialize<T>(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid parameters...");
            }

            XmlSerializer ser = new XmlSerializer(typeof(T));
            StreamReader stream = new StreamReader(path);
            try
            {
                T result = (T)ser.Deserialize(stream);
                stream.Close();

                return result;
            }
            catch (Exception ex)
            {
                stream.Close();

                throw ex;
            }
        }
    }
}
