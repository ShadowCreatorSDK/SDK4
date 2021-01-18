using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {


    /// <summary>
    /// Event 委托
    /// </summary>
    /// <param name="aEvent"></param>
    /// <param name="handShankPart"></param>
    public delegate void EventDelegate(GCEventType aEvent, InputDeviceGCPart GCPart);

}
