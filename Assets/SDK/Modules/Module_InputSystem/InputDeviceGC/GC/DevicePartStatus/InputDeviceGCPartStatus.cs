using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {


public abstract class InputDeviceGCPartStatus : InputDevicePartStatusBase {

    public InputDeviceGCPart inputDeviceGCPart;
        public InputDeviceGCPartStatus(InputDeviceGCPart inputDeviceGCPart) : base(inputDeviceGCPart) {
            this.inputDeviceGCPart = inputDeviceGCPart;
        }

    }
}
