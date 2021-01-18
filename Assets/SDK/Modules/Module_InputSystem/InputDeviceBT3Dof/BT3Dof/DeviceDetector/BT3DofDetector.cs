using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class BT3DofDetector : GCDetector {

        public InputDeviceBT3DofPart inputDeviceBT3DofPart {
            get {
                return inputDevicePartBase as InputDeviceBT3DofPart;
            }
        }

        public BT3DofPointer bT3DofPointer {
            get {
                return Transition<BT3DofPointer>(pointerBase);
            }
        }

    }
}
