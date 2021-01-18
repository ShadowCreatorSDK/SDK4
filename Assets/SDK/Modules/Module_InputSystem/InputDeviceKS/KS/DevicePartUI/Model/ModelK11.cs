using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class ModelK11 : ModelGCBase {
        public InputDeviceKSPartUI inputDeviceKSPartUI {
            get {
                return inputDevicePartUIBase as InputDeviceKSPartUI;
            }
        }

        [Header("3DofBias")]
        public Vector3 modelPositionDeltaWithDevice = new Vector3(0, 0, 0f);

        [Header("Keys")]
        public MeshRenderer Upkey;
        public MeshRenderer Downkey;
        public MeshRenderer LeftKey;
        public MeshRenderer RightKey;
        public MeshRenderer TriggerKey;

        [Header("Joystick")]
        public Transform joystick;

        [Range(0,4)]
        public float rotationfactor = 2;
        public Vector2 joystickInitalValue = new Vector2(8, 8);
        public Vector3 joystickInitallocalEulerAngles;


        [Header("MaterialVisual")]
        public Material pressMaterial;
        public Material releaseMaterial;


        void UpdateTransform() {
            transform.localPosition = modelPositionDeltaWithDevice;
        }

        public override void OnSCStart() {
            base.OnSCStart();

        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateTransform();
            UpdateJoystickTransform();
        }

        //[Range(0, 16)]
        //public int xx = 8;
        //[Range(0, 16)]
        //public int yy = 8;
        Vector3 biasJoystick = new Vector3(0,0,0);
        public virtual void UpdateJoystickTransform() {

            if (joystick) {

                if(inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.JoystickX != joystickInitalValue.x || inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.JoystickY != joystickInitalValue.y) {
                    biasJoystick.z = rotationfactor * (inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.JoystickX - joystickInitalValue.x);
                    biasJoystick.x = rotationfactor * (inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.JoystickY - joystickInitalValue.y);
                    joystick.localEulerAngles = joystickInitallocalEulerAngles + biasJoystick;
                }
            }
        }

        public override void SetHandleKeysColor() {
            if (releaseMaterial == null || pressMaterial == null) {
                return;
            }

            foreach (var item in inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.inputKeys.inputKeyDic) {

                if(TriggerKey && (item.Key == InputKeyCode.LTrigger || item.Key == InputKeyCode.RTrigger)) {
                    TriggerKey.material = releaseMaterial;
                    if(item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        TriggerKey.material = pressMaterial;
                    }
                } else if (Upkey && (item.Key == InputKeyCode.RFunction || item.Key == InputKeyCode.LFunction)) {
                    Upkey.material = releaseMaterial;
                    if (item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        Upkey.material = pressMaterial;
                    }
                } else if(RightKey && (item.Key == InputKeyCode.RFunction || item.Key == InputKeyCode.LFunction)) {
                    RightKey.material = releaseMaterial;
                    if(item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        RightKey.material = pressMaterial;
                    }
                } else if (Downkey && (item.Key == InputKeyCode.X || item.Key == InputKeyCode.A)) {
                    Downkey.material = releaseMaterial;
                    if (item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        Downkey.material = pressMaterial;
                    }
                } else if(LeftKey && (item.Key == InputKeyCode.Y || item.Key == InputKeyCode.B)) {
                    LeftKey.material = releaseMaterial;
                    if(item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                        LeftKey.material = pressMaterial;
                    }
                }

            }
        }


    }
}
