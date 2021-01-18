using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class KSDetector : GCDetector {

        public InputDeviceKSPart inputDeviceKSPart {
            get {
                return inputDevicePartBase as InputDeviceKSPart;
            }
        }

        public KSPointer bT3DofPointer {
            get {
                return Transition<KSPointer>(pointerBase);
            }
        }

    }
}
