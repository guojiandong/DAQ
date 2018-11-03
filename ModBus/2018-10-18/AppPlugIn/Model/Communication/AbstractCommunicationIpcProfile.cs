using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{    
    [Serializable]
    public abstract class AbstractCommunicationIpcProfile : CommunicationSocketProfile
    {
        public AbstractCommunicationIpcProfile() : this("")
        {
        }

        public AbstractCommunicationIpcProfile(string tag) : base(tag)
        {
        }

        public AbstractCommunicationIpcProfile(string tag, int port) : base(tag, port)
        {
            
        }

        public AbstractCommunicationIpcProfile(AbstractCommunicationIpcProfile other) : this(other.Tag, other.Port)
        {
            CopyFrom(other);
        }

        public void CopyFrom(AbstractCommunicationIpcProfile other)
        {
            base.CopyFrom(other);
        }

        //public override AbstractCommunicationProfile Clone()
        //{
        //    return new CommunicationIpcClientProfile(this);
        //}
    }
}
