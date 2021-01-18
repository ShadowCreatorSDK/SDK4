using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {
    public class HeadDetector : DetectorBase {

        public InputDeviceHeadPart inputDevicePartHead {
            get {
                return inputDevicePartBase as InputDeviceHeadPart;
            }
        }

        public HeadPointer headPointer {
            get {
                return pointerBase as HeadPointer;
            }
        }

    }
}
