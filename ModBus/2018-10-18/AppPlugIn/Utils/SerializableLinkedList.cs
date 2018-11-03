using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Utils
{
    [Serializable]
    public class SerializableLinkedList<T> : LinkedList<T>, ISerializable, IXmlSerializable
    {
        public SerializableLinkedList() : base()
        {
        }

        public SerializableLinkedList(IEnumerable<T> collection) : base(collection)
        {
        }

        // Implied by ISerializable, but interfaces can't actually define constructors:
        SerializableLinkedList(SerializationInfo info, StreamingContext context)
            : base((IEnumerable<T>)info.GetValue("value", typeof(List<T>)))
        {
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", this.ToList());
        }

        public XmlSchema GetSchema()
        {
            //throw new NotImplementedException();

            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;

            XmlSerializer ValueSerializer = new XmlSerializer(typeof(T));

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //reader.ReadStartElement("SerializableLinkedList");
                reader.ReadStartElement(typeof(T).Name);
                //TKey tk = (TKey)KeySerializer.Deserialize(reader);
                //reader.ReadEndElement();
                //reader.ReadStartElement("value");
                T vl = (T)ValueSerializer.Deserialize(reader);
                //reader.ReadEndElement();
                reader.ReadEndElement();
                this.AddLast(vl);
                reader.MoveToContent();
            }

            //reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            //XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(T));

            //writer.WriteStartElement("SerializableLinkedList");

            LinkedListNode<T> node = this.First;
            while(node != null)
            {
                writer.WriteStartElement(typeof(T).Name);
                ValueSerializer.Serialize(writer, node.Value);
                writer.WriteEndElement();

                node = node.Next;
            }

            //writer.WriteEndElement();

            //foreach (KeyValuePair<TKey, TValue> kv in this)
            //{
                
            //    write.WriteStartElement("key");
            //    KeySerializer.Serialize(write, kv.Key);
            //    write.WriteEndElement();
            //    write.WriteStartElement("value");
            //    ValueSerializer.Serialize(write, kv.Value);
            //    write.WriteEndElement();
            //    write.WriteEndElement();
            //}
        }
    }
}
