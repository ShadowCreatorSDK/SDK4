using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {

    public class KSPointer : GCPointer {
        public KSDetector KSDetector {
            get {
                return detectorBase as KSDetector;
            }
        }

    }
}
