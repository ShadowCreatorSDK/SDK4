using System.Collections;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class InputDeviceGGT26DofPartUI : InputDeviceHandPartUI {
        public InputDeviceGGT26DofPart inputDeviceGGT26DofPart {
            get {
                return inputDevicePartBase as InputDeviceGGT26DofPart;
            }
        }

        public ModelGGT26Dof modelGGT26Dof {
            get {
                return modelBase as ModelGGT26Dof;
            }
        }



    }
}