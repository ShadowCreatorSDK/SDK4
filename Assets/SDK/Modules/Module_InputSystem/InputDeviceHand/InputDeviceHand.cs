using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class InputDeviceHand : InputDeviceBase {

        [Header("Show Hands Information")]
        public bool ShowHandsInfo = false;
        public TextMesh handsText;

        [Header("Enable Hands")]
        public bool LeftHandOpen = false;
        public bool RightHandOpen = true;

        [SerializeField]
        private bool EditorHideRight = true;

        public override InputDeviceType inputDeviceType {
            get {
                return InputDeviceType.UnKnow;
            }
        }

        protected override void InputDeviceStart() {
            SetActiveInputDevicePart(InputDevicePartType.HandLeft, LeftHandOpen);

            if(Application.platform == RuntimePlatform.Android)
                SetActiveInputDevicePart(InputDevicePartType.HandRight, RightHandOpen);
            else if(EditorHideRight == false) {
                SetActiveInputDevicePart(InputDevicePartType.HandRight, EditorHideRight == true? false:RightHandOpen);
            }
        }


    }
}
