using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Common.Attr
{
    [Guid("FD6B34AC-ABE1-4E26-8A06-6BD1CE10F013")]
    [Serializable]
    public class EnumDescriptionAttribute : Attribute
    {
        private string _Description;
        public EnumDescriptionAttribute(string description)
        {
            _Description = description;
        }

        public string Description
        {
            get { return _Description; }
        }

        public static string GetEnumDescription(Enum enumobj)
        {
            System.Reflection.FieldInfo fieldInfo = enumobj.GetType().GetField(enumobj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            if (attribArray.Length == 0)
            {
                return enumobj.ToString();
            }
            else
            {
                EnumDescriptionAttribute attrib = attribArray[0] as EnumDescriptionAttribute;
                return attrib.Description;
            }
        }
    }
}
