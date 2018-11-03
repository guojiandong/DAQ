using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ksat.AppPlugIn.Model
{
    [Guid("DC3ED101-FC01-4C70-A969-A64DB4231FC7")]
    public sealed class PermissionManager
    {
        private static PermissionManager sInstance;

        public static PermissionManager Instance()
        {
            lock (typeof(PermissionManager))
            {
                if (sInstance == null)
                    sInstance = new PermissionManager();
            }

            return sInstance;
        }

        private LinkedList<PermissionInfo> mPermissions;

        private PermissionManager()
        {
            this.mPermissions = new LinkedList<PermissionInfo>();
        }

        public void SetPermission(RoleTypeEnum role, string permission, params AccessTypeEnum[] rights)
        {
            if (String.IsNullOrEmpty(permission))
            {
                throw new ArgumentNullException("permission can't be empty...");
            }

            LinkedListNode<PermissionInfo> node = mPermissions.First;
            while (node != null)
            {
                if (node.Value.Role == role && node.Value.Permission == permission)
                {
                    node.Value.SetPermission(rights);
                    break;
                }

                node = node.Next;
            }
        }

        public void RemovePermission(RoleTypeEnum role, string permission, params AccessTypeEnum[] rights)
        {
            if (String.IsNullOrEmpty(permission))
            {
                throw new ArgumentNullException("permission can't be empty...");
            }

            LinkedListNode<PermissionInfo> node = mPermissions.First;
            while (node != null)
            {
                if (node.Value.Role == role && node.Value.Permission == permission)
                {
                    node.Value.RemovePermission(rights);
                    break;
                }

                node = node.Next;
            }
        }

        public bool HasVisiablePermission(RoleTypeEnum role, string permission)
        {
            return this.HasPermission(role, permission, AccessTypeEnum.Visiable);
        }

        public bool HasOpenPermission(RoleTypeEnum role, string permission)
        {
            return this.HasPermission(role, permission, AccessTypeEnum.Open);
        }

        public bool HasClosePermission(RoleTypeEnum role, string permission)
        {
            return this.HasPermission(role, permission, AccessTypeEnum.Close);
        }


        public bool HasPermission(RoleTypeEnum role, string permission, AccessTypeEnum right)
        {
            if(String.IsNullOrEmpty(permission) || right == AccessTypeEnum.None)
            {
                return false;
            }

            LinkedListNode<PermissionInfo> node = mPermissions.First;
            while(node != null)
            {
                if(node.Value.Role == role && node.Value.Permission == permission)
                {
                    return node.Value.HasPermission(right);
                }

                node = node.Next;
            }

            return false;
        }
    }
}
