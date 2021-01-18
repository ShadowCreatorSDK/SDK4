using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// 声明AnyKeyEventDelegate委托类型,所有输入设备公用
    /// </summary>
    public delegate void AnyKeyEventDelegate(InputKeyCode keyCode, InputDevicePartBase part);
}
