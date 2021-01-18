using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {

    public class InputDevicePartDispatchEventBT3Dof : InputDevicePartDispatchEventGC {

        public InputDeviceBT3DofPart inputDeviceBT3DofPart;
        public InputDevicePartDispatchEventBT3Dof(InputDeviceBT3DofPart inputDeviceBT3DofPart) : base(inputDeviceBT3DofPart) {
            this.inputDeviceBT3DofPart = inputDeviceBT3DofPart;
        }
    }
}
