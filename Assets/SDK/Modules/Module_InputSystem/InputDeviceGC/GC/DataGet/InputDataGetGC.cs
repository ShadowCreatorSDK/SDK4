using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public abstract class InputDataGetGC : InputDataGetBase {

        public InputDeviceGCPart inputDeviceGCPart;

        public InputDataGetGCPosture inputDataGetGCPosture;
        public InputDataGetGCKey inputDataGetGCKey;
        public InputDataGetGCTouch inputDataGetGCTouch;
        public InputDataGetGCIMU inputDataGetGCIMU;
        public InputDataGetGC(InputDeviceGCPart inputDeviceGCPart) :base(inputDeviceGCPart) {
            this.inputDeviceGCPart = inputDeviceGCPart;
        }
    }
}
