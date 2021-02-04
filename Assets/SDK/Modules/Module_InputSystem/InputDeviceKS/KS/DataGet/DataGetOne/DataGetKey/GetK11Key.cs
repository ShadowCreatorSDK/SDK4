using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class GetK11Key : SCModule {

        public InputDataGetKSKey inputDataGetKSKey;
        public GetK11Key(InputDataGetKSKey inputDataGetKSKey) {
            this.inputDataGetKSKey = inputDataGetKSKey;
        }

        public override void OnSCAwake() {
            base.OnSCAwake();
            if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K11_Left_EnterKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K11_Left_EnterKeyAlias", "LTrigger"), false, out keyparse) ? keyparse : InputKeyCode.LTrigger;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = InputKeyCode.LTrigger;
                }
            } else if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K11_Right_EnterKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K11_Right_EnterKeyAlias", "RTrigger"), false, out keyparse) ? keyparse : InputKeyCode.RTrigger;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = InputKeyCode.RTrigger;
                }
            }
            DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " inputDataGetKSKey.EnterKeyAlias:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias,this,true);

            if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K11_Left_CalibrationKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K11_Left_CalibrationKeyAlias", "DOWN"), false, out keyparse) ? keyparse : InputKeyCode.DOWN;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = InputKeyCode.DOWN;
                }
            } else if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K11_Right_CalibrationKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K11_Right_CalibrationKeyAlias", "A"), false, out keyparse) ? keyparse : InputKeyCode.A;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = InputKeyCode.A;
                }
            }
            DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " inputDataGetKSKey.CalibrationKeyAlias:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias, this, true);
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            ProcessKeyList();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            inputDataGetKSKey = null;
        }


        protected virtual void ProcessKeyList() {

            if(InputDataGC.GCData.GCKeyList.Count > 0) {

                InputKeyCode inputKeyCode;
                InputKeyState inputKeyState;

                if((inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft && InputDataGC.GCData.GCKeyList[0].deivceID == 0)
                    ||
                   (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight && InputDataGC.GCData.GCKeyList[0].deivceID == 1)) {

                    switch((KSKeyCode)InputDataGC.GCData.GCKeyList[0].keycode) {

                        case KSKeyCode.K11_A:
                            inputKeyCode = InputKeyCode.A;
                            break;
                        case KSKeyCode.K11_B:
                            inputKeyCode = InputKeyCode.B;
                            break;
                        case KSKeyCode.K11_X:
                            inputKeyCode = InputKeyCode.X;
                            break;
                        case KSKeyCode.K11_Y:
                            inputKeyCode = InputKeyCode.Y;
                            break;
                        case KSKeyCode.K11_RTrigger:
                            inputKeyCode = InputKeyCode.RTrigger;
                            break;


                        case KSKeyCode.K11_DOWN:
                            inputKeyCode = InputKeyCode.DOWN;
                            break;
                        case KSKeyCode.K11_UP:
                            inputKeyCode = InputKeyCode.UP;
                            break;
                        case KSKeyCode.K11_LEFT:
                            inputKeyCode = InputKeyCode.LEFT;
                            break;
                        case KSKeyCode.K11_RIGHT:
                            inputKeyCode = InputKeyCode.RIGHT;
                            break;
                        case KSKeyCode.K11_LTrigger:
                            inputKeyCode = InputKeyCode.LTrigger;
                            break;

                        default:
                            inputKeyCode = InputKeyCode.OTHER;
                            break;
                    }

                    switch((KSKeyState)InputDataGC.GCData.GCKeyList[0].keyevent) {
                        case KSKeyState.UP:
                            inputKeyState = InputKeyState.UP;
                            break;
                        case KSKeyState.DOWN:
                            inputKeyState = InputKeyState.DOWN;
                            break;
                        case KSKeyState.LONG:
                            inputKeyState = InputKeyState.LONG;
                            break;
                        default:
                            inputKeyState = InputKeyState.Null;
                            break;
                    }

                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.inputKeys.InputDataAddKey(inputKeyCode, inputKeyState);
                    DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " ProcessKeyList:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " Add Key:" + inputKeyCode + "  State:" + inputKeyState, this, true);

                    ///Enter别名
                    if(inputKeyCode == inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.EnterKeyAlias) {
                        inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.inputKeys.InputDataAddKey(InputKeyCode.Enter, inputKeyState);
                        DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " ProcessKeyList:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " Add (Alias:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.EnterKeyAlias + ") Key:" + InputKeyCode.Enter + "  State:" + inputKeyState, this, true);
                    }

                    if (inputKeyCode == inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.CancelKeyAlias) {
                        inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.inputKeys.InputDataAddKey(InputKeyCode.Cancel, inputKeyState);
                        DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " ProcessKeyList:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " Add (Alias:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.CancelKeyAlias + ") Key:" + InputKeyCode.Cancel + "  State:" + inputKeyState, this, true);
                    }


                    InputDataGC.GCData.GCKeyList.RemoveAt(0);

                }

            }
        }


    }
}
