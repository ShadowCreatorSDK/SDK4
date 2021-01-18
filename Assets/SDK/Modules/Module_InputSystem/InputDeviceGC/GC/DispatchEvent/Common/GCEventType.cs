using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;


namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    /// <summary>
    /// Event类型，注意Event触发后，下一帧LateUpdate将复位
    /// </summary>
    public enum GCEventType {

        TouchSlideLeft,
        TouchSlideRight,
        TouchSlideUp,
        TouchSlideDown,

        TouchDown, 
        TouchDrag,
        TouchUp,

        Null,

    }

}







