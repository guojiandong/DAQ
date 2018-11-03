using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ksat.AppPlugIn.Model
{
    [Serializable]
    [Guid("F39A8345-0B90-4AE4-9FD3-14261F0579BB")]
    public class PermissionInfo
    {
        public readonly RoleTypeEnum Role;

        public readonly string Permission;

        private UInt32 Right;

        public PermissionInfo(RoleTypeEnum role, string perm, params AccessTypeEnum[] rights)
        {
            this.Role = role;
            this.Permission = perm;
            this.Right = (UInt32)AccessTypeEnum.None;

            this.SetPermission(rights);
        }

        internal void SetPermission(params AccessTypeEnum[] rights)
        {
            if (rights != null)
            {
                foreach (AccessTypeEnum item in rights)
                {
                    this.Right |= (UInt32)item;
                }
            }
        }

        internal void RemovePermission(params AccessTypeEnum[] rights)
        {
            if (rights != null)
            {
                UInt32 r = 0;
                foreach (AccessTypeEnum item in rights)
                {
                    r |= ((UInt32)item);
                }

                this.Right &= ~r;
            }
        }

        public bool HasPermission(AccessTypeEnum right)
        {
            if (right == AccessTypeEnum.None)
                return false;

            UInt32 r = (UInt32)right;

            if ((Right & r) == r)
            {
                return true;
            }

            return false;
        }
    }
}
