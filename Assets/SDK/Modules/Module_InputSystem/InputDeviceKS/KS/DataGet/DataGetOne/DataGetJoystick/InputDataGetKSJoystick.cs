using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDataGetKSJoystick : InputDataGetOneBase {
        public InputDataGetKS inputDataGetKS;
        public InputDataGetKSJoystick(InputDataGetKS _inputDataGetKS) : base(_inputDataGetKS) {
            inputDataGetKS = _inputDataGetKS;
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateJoystickData();
        }

        public virtual void UpdateJoystickData() {
            if(InputDataKS.TempJoystickDataList.Count > 0) {

                if((inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft && InputDataKS.TempJoystickDataList[0].deviceID == 0)
                    ||
                   (inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight && InputDataKS.TempJoystickDataList[0].deviceID == 1)) {

                    inputDataGetKS.inputDeviceKSPart.inputDataKS.JoystickX = (int)Mathf.Lerp(inputDataGetKS.inputDeviceKSPart.inputDataKS.JoystickX, InputDataKS.TempJoystickDataList[0].JoystickX, 0.4f);
                    inputDataGetKS.inputDeviceKSPart.inputDataKS.JoystickY = (int)Mathf.Lerp(inputDataGetKS.inputDeviceKSPart.inputDataKS.JoystickY, InputDataKS.TempJoystickDataList[0].JoystickY, 0.4f);

                    InputDataKS.TempJoystickDataList.RemoveAt(0);

                    DebugMy.Log(inputDataGetKS.inputDeviceKSPart.PartType + " UpdateJoystickData:" + inputDataGetKS.inputDeviceKSPart.inputDataKS.JoystickX +" "+ inputDataGetKS.inputDeviceKSPart.inputDataKS.JoystickY, this, true);

                }

            }
        }
    }
}
