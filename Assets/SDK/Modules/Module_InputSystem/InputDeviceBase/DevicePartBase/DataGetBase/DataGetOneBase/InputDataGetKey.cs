using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public class InputDataGetKey : InputDataGetOneBase {

        public InputDataGetKey(InputDataGetBase _inputDataGet) : base(_inputDataGet) {
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            OnUpdateKey();
        }

        public virtual void OnUpdateKey() {

            if(SvrPlugin.Instance == null)
                return;

            if(SvrPlugin.Instance.HeadSetEnterKeyDown()) {
                inputDataGetBase.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Enter, InputKeyState.DOWN);
            } else if(SvrPlugin.Instance.HeadSetEnterKeyUp()) {
                inputDataGetBase.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Enter, InputKeyState.UP);
            }

            if(SvrPlugin.Instance.HeadSetBackKeyDown()) {
                inputDataGetBase.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Back, InputKeyState.DOWN);
            } else if(SvrPlugin.Instance.HeadSetBackKeyUp()) {
                inputDataGetBase.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Back, InputKeyState.UP);
            }
        }
    }
}
