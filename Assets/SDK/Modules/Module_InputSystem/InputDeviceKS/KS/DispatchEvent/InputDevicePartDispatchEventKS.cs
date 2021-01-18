using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {

    public class InputDevicePartDispatchEventKS : InputDevicePartDispatchEventGC {

        public InputDeviceKSPart inputDeviceKSPart;
        public InputDevicePartDispatchEventKS(InputDeviceKSPart inputDeviceKSPart) : base(inputDeviceKSPart) {
            this.inputDeviceKSPart = inputDeviceKSPart;
        }
    }
}
