using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public class GCPointer : PointerBase {
        public GCDetector gcDetector {
            get {
                return detectorBase as GCDetector;
            }
        }

        public override PointerType PointerType => PointerType.Far;

        protected override void UpdateTransform() {
            transform.position = gcDetector.inputDeviceGCPart.inputDeviceGCPartUI.ModelGC.StartPoint.position;
            transform.rotation = gcDetector.inputDeviceGCPart.inputDeviceGCPartUI.ModelGC.StartPoint.rotation;
        }
    }
}
