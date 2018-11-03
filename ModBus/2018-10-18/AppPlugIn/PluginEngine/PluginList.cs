using Ksat.AppPlugIn.Logging;
using System;
namespace Ksat.AppPlugIn.PluginEngine
{
    [System.Runtime.InteropServices.Guid("133FA7F0-B021-4C8B-BB3E-1DDBA7FF2399")]
    public class PluginList : System.Collections.Generic.LinkedList<PluginInfo>
    {
        /// <summary>
        /// 对集合执行操作
        /// </summary>
        /// <param name="action">要执行的函数</param>
        public void ProviderExecute(Action<IPlugin> action)
        {
#if true
            System.Collections.Generic.LinkedListNode<PluginInfo> node = this.First;
            while (node != null)
            {
                if (node.Value.PluginProvider == null) continue;
                action(node.Value.PluginProvider);

                node = node.Next;
            }
#else
            this.ForEach(s =>
            {
                if (s.PluginProvider == null) return;
                action(s.PluginProvider);
            });
#endif
        }

        public PluginInfo GetPlugin(string pluginname)
        {
            System.Collections.Generic.LinkedListNode<PluginInfo> node = this.First;
            while (node != null)
            {
                if (String.Compare(node.Value.TypeName, pluginname, true) == 0)
                    return node.Value;

                node = node.Next;
            }

            return null;
        }

        public bool CheckExisting(string pluginname)
        {
            System.Collections.Generic.LinkedListNode<PluginInfo> node = this.First;
            while(node != null)
            {
                if (String.Compare(node.Value.TypeName, pluginname, true) == 0)
                    return true;

                node = node.Next;
            }

            return false;
        }

        public void AddPlugin(PluginInfo[] slist)
        {
            if (slist == null || slist.Length == 0)
                return;

            foreach(PluginInfo item in slist)
            {
                if (CheckExisting(item.TypeName))
                    continue;

                this.AddLast(item);
            }
        }

        private void MoveNode(System.Collections.Generic.LinkedListNode<PluginInfo> curentNode, string depandname)
        {
            bool found = false;
            System.Collections.Generic.LinkedListNode<PluginInfo> node = curentNode.Next;
            while (node != null)
            {
                if (String.Compare(node.Value.Assembly, depandname, true) == 0)
                {
                    found = true;
                    this.Remove(node);
                    curentNode = this.AddBefore(curentNode, node.Value);
                    break;
                }

                node = node.Next;
            }

            if (found && curentNode.Value.Depands.Count > 0)
            {
                foreach (string s in curentNode.Value.Depands)
                {
                    this.MoveNode(curentNode, s);
                }
            }
        }

        public void ReSort()
        {
            System.Collections.Generic.LinkedListNode<PluginInfo> node = this.First;
            while (node != null)
            {
                if (node.Value.Depands.Count > 0)
                {
                    foreach (string s in node.Value.Depands)
                    {
                        this.MoveNode(node, s);
                    }
                }

                node = node.Next;
            }
        }

        public void Dump()
        {
            System.Collections.Generic.LinkedListNode<PluginInfo> node = this.First;
            while (node != null)
            {
                Logger.Info(this.GetType().Name, node.Value.ToString());

                node = node.Next;
            }
        }
    }
}
