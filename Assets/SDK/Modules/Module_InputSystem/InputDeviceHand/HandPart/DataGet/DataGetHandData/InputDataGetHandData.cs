
using UnityEngine;

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class InputDataGetHandsData : InputDataGetOneBase {

        public InputDataGetHand inputDataGetHand;
        public InputDataGetHandsData(InputDataGetHand inputDataGetHand) : base(inputDataGetHand) {
            this.inputDataGetHand = inputDataGetHand;
        }

    }
}