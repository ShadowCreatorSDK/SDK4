using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {

    public class InputDataHead : InputDataBase {

        public InputDeviceHeadPart inputDeviceHeadPart;
        public InputDataHead(InputDeviceHeadPart inputDeviceHeadPart) :base(inputDeviceHeadPart) {
            this.inputDeviceHeadPart = inputDeviceHeadPart;
        }


    }
}
