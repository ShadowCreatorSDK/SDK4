using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class ModelK07 : ModelBT3DofBase {
        
        public MeshRenderer volumeDown;
        public MeshRenderer volumeUp;
        public MeshRenderer tpKey;

        public override void SetHandleKeysColor() {
            base.SetHandleKeysColor();
            if (releaseMaterial == null || pressMaterial == null) {
                return;
            }

            foreach (var item in inputDeviceBT3DofPartUI.inputDeviceBT3DofPart.inputDataBT3Dof.inputKeys.inputKeyDic) {

                if (item.Key == InputKeyCode.Tp) {
                    tpKey.material = releaseMaterial;
                    if (item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        tpKey.material = pressMaterial;
                    }
                } else if (item.Key == InputKeyCode.VolumeDown) {
                    volumeDown.material = releaseMaterial;
                    if (item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        volumeDown.material = pressMaterial;
                    }
                } else if (item.Key == InputKeyCode.VolumeUp) {
                    volumeUp.material = releaseMaterial;
                    if (item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        volumeUp.material = pressMaterial;
                    }
                }

            }
        }
    }
}