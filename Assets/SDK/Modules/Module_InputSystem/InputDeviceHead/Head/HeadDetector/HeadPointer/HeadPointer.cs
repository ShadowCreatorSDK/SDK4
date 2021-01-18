using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {

    public class HeadPointer : PointerBase {
        public HeadDetector headDetector {
            get {
                return detectorBase as HeadDetector;
            }
        }

        public override PointerType PointerType => PointerType.Far;
    }
}
