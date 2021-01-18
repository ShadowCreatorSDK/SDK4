using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {

    public class BT3DofPointer : GCPointer {
        public BT3DofDetector bT3DofDetector {
            get {
                return detectorBase as BT3DofDetector;
            }
        }

    }
}
