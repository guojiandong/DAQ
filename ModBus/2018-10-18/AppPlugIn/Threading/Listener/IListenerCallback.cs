using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Listener
{
    [Guid("89F5A259-E16E-4BEE-A810-390F6336985B")]
    public interface IListenerCallback<TParam>
    {
        void onCallback(object sender, TParam args);
    }

    [Guid("353AD346-0170-4242-8798-DA980468CC2D")]
    public interface IListenerCallbackEventArgs : IListenerCallback<EventArgs>
    {
    }

    [Guid("E2D5B9CF-0498-47B5-A212-4F4A2C6BB514")]
    public interface IListenerCallbackBooleanParam : IListenerCallback<bool>
    {
    }

    [Guid("10C32A6F-52F4-43D7-8B89-627817ACD56A")]
    public interface IListenerCallbackIntParam : IListenerCallback<int>
    {
    }

    [Guid("56AF6673-504C-4742-B9B0-008D4CD77351")]
    public interface IListenerCallbackObjectParam : IListenerCallback<object>
    {
    }

    [Guid("C14A1C9F-663E-4553-BC5C-52F438CB9A59")]
    public interface IListenerCallbackStringParam : IListenerCallback<string>
    {
    }
}
