using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugInUiLibrary.Login
{
    public interface ILoginResultListener
    {
        void onLoginResult(string loginid, AccountType t);
    }

    public interface ISimpleLoginResultListener
    {
        void onLoginResult(bool flag);
    }
}
