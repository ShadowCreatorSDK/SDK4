using SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {

    public class InputDeviceHeadPartStatus : InputDevicePartStatusBase {

        public InputDeviceHeadPart inputDeviceHeadPart;
        public InputDeviceHeadPartStatus(InputDeviceHeadPart _inputDeviceHeadPart) : base(_inputDeviceHeadPart) {
            inputDeviceHeadPart = _inputDeviceHeadPart;
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            
            if(Module_InputSystem.instance == null)
                return;

            inputDeviceHeadPart.inputDataBase.isVaild = ! Module_InputSystem.instance.IsSomeDeviceActiveWithoutHead;
        }
    }
}
