using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {

    public class InputDevicePartDispatchEventHead : InputDevicePartDispatchEventBase {

        public InputDeviceHeadPart inputDeviceHeadPart;
        public InputDevicePartDispatchEventHead(InputDeviceHeadPart inputDeviceHeadPart) : base(inputDeviceHeadPart) {
            this.inputDeviceHeadPart = inputDeviceHeadPart;
        }
    }
}
