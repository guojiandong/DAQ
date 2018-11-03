using Ksat.AppPlugIn.Utils.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils
{
    public sealed class SerializerHelper
    {
        public enum SerializerMode
        {
            Xml,
            Binary,
            Memory
        }

        public static void Serializer(object obj, string path, SerializerMode mode)
        {
            if (obj == null || String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid parameters...");
            }

            switch (mode)
            {
                case SerializerMode.Xml:
                    new XmlSerializerImpl().Serializer(obj, path);
                    break;
                case SerializerMode.Binary:
                    new FileBinarySerializer().Serializer(obj, path);
                    break;
                default:
                    throw new NotSupportedException("Not support the mode:" + mode);
            }
        }

        public static T Deserialize<T>(string path, SerializerMode mode)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid parameters...");
            }

            switch (mode)
            {
                case SerializerMode.Xml:
                    return new XmlSerializerImpl().Deserialize<T>(path);
                case SerializerMode.Binary:
                    return new FileBinarySerializer().Deserialize<T>(path);
                default:
                    throw new NotSupportedException("Not support the mode:" + mode);
            }
        }


        //private class XmlSerializerImpl
        //{            

        //}


        /// <summary>
        /// 序列化BIN
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void BinarySerialize<T>(string filePath, T t) where T : class
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, t);
            }
        }

        /// <summary>
        /// 反序列化BIN
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                T t = default(T);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter formatter = new BinaryFormatter();
                    t = (T)formatter.Deserialize(fs);
                }
                return t;
            }
            else
            {
                return default(T);
            }
        }
#if false
        /// <summary>
        /// 序列化SOAP
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void SoapSerialize<T>(string filePath, T t) where T : class
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fs, t);
            }
        }


        /// <summary>
        /// 反序列化SOAP
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T SoapDeserialize<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                T t = default(T);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter formatter = new SoapFormatter();
                    t = (T)formatter.Deserialize(fs);
                }
                return t;
            }
            else
            {
                return default(T);
            }
        }
#endif
        /// <summary>
        /// 序列化XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void XmlSerialize<T>(string filePath, T t)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                ser.Serialize(writer, t);
            }
        }


        /// <summary>
        /// 反序列化XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T XmlDeserailize<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                StreamReader read = new StreamReader(filePath);
                T t = (T)ser.Deserialize(read);
                read.Close();
                read.Dispose();
                read = null;
                return t;
            }
            else
            {
                return default(T);
            }
        }

        public static void JsonSerialize<T>(string filePath, T t) where T : class
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(t);
            System.IO.File.WriteAllText(filePath, json);
        }

        public static T JsonDeserialize<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Deserialize<T>(json);
            }
            else
            {
                return default(T);
            }
        }
    }
}
