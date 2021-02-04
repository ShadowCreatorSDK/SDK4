using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public abstract class InputDataGetGCPosture : InputDataGetOneBase {
        public InputDataGetGC inputDataGetGC;

        public InputDataGetGCPosture(InputDataGetGC _inputDataGetGC) : base(_inputDataGetGC) {
            inputDataGetGC = _inputDataGetGC;
        }

        public PostureType postureType {
            get {
                return inputDataGetGC.inputDeviceGCPart.PostureType;
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateRotation();
            UpdatePosition();
        }

        protected abstract void UpdatePosition();

        protected abstract void UpdateRotation();

    }
}
