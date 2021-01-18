using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class ModelGGT26Dof : ModelHand {
        public InputDeviceGGT26DofPartUI inputDeviceGGT26DofPartUI {
            get {
                return inputDevicePartUIBase as InputDeviceGGT26DofPartUI;
            }
        }

    }
}