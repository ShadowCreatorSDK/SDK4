using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDeviceHandPartStatus : InputDevicePartStatusBase {

        public InputDeviceHandPart inputDeviceHandPart;
        public InputDeviceHandPartStatus(InputDeviceHandPart _inputDeviceHandPart) : base(_inputDeviceHandPart) {
            inputDeviceHandPart = _inputDeviceHandPart;
        }

    }
}
