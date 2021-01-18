using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public class GCDetector : DetectorBase {

        public InputDeviceGCPart inputDeviceGCPart {
            get {
                return inputDevicePartBase as InputDeviceGCPart;
            }
        }

        public GCPointer gcPointer {
            get {
                return Transition<GCPointer>(pointerBase);
            }
        }

    }
}
