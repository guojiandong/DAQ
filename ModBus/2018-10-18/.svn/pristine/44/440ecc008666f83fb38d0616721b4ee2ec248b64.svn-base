using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ksat.AppPlugIn.Model
{
    [Serializable]
    [Guid("38EAAA3A-5B10-4311-BE8F-8127D68913DE")]
    public class UserInfo
    {
        public string Name { get; private set; }

        public RoleTypeEnum Role { get; private set; }

        public long LoginTime { get; private set; }

        public UserInfo(string name, RoleTypeEnum role)
        {
            this.Name = name;
            this.Role = role;
            this.LoginTime = DateTime.Now.Ticks;
        }
    }
}
