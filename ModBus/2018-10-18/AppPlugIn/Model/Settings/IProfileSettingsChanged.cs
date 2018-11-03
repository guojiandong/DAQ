using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Settings
{
    public interface IProfileSettingsChanged
    {
        void onProfileSettingsChanged(AbstractProfileSettings current);
    }
}
