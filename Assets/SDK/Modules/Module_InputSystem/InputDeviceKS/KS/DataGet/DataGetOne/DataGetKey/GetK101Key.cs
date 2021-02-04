using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class GetK101Key : SCModule {

        public InputDataGetKSKey inputDataGetKSKey;
        public GetK101Key(InputDataGetKSKey inputDataGetKSKey) {
            this.inputDataGetKSKey = inputDataGetKSKey;
        }


        public int Meansurebias = 2;

        public int HallFowardPressValue = 3;
        public bool IsHallFowardPress = false;

        public int HallInsidePressValue = 3;
        public bool IsHallInsidePress = false;
        public override void OnSCAwake() {
            base.OnSCAwake();
            if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K101_Left_EnterKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K101_Left_EnterKeyAlias", "LHallForward"), false, out keyparse) ? keyparse : InputKeyCode.LHallForward;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = InputKeyCode.LHallForward;
                }
            } else if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K101_Right_EnterKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K101_Right_EnterKeyAlias", "RHallForward"), false, out keyparse) ? keyparse : InputKeyCode.RHallForward;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias = InputKeyCode.RHallForward;
                }
            }
            DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " inputDataGetKSKey.EnterKeyAlias:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.EnterKeyAlias, this, true);

            if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K101_Left_CalibrationKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K101_Left_CalibrationKeyAlias", "LFunction"), false, out keyparse) ? keyparse : InputKeyCode.LFunction;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = InputKeyCode.LFunction;
                }
            } else if (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "K101_Right_CalibrationKeyAlias")) {
                    InputKeyCode keyparse;
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = System.Enum.TryParse<InputKeyCode>(API_Module_SDKConfiguration.GetString("Module_InputSystem", "K101_Right_CalibrationKeyAlias", "RFunction"), false, out keyparse) ? keyparse : InputKeyCode.RFunction;
                } else {
                    inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias = InputKeyCode.RFunction;
                }
            }
            DebugMy.Log(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType + " inputDataGetKSKey.CalibrationKeyAlias:" + inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataGC.CalibrationKeyAlias, this, true);
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            HallConvertToKey();
            ProcessKeyList();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            inputDataGetKSKey = null;
        }

        protected virtual void HallConvertToKey() {


            if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.HallInside <= HallInsidePressValue && IsHallInsidePress == false) {
                IsHallInsidePress = true;

                if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.LHallInside, (int)KSKeyState.DOWN, (int)KSIndex.Left);
                } else if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.RHallInside, (int)KSKeyState.DOWN, (int)KSIndex.Right);
                }

            } else if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.HallInside > HallInsidePressValue + Meansurebias && IsHallInsidePress == true) {
                IsHallInsidePress = false;

                if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.LHallInside, (int)KSKeyState.UP, (int)KSIndex.Left);
                } else if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.RHallInside, (int)KSKeyState.UP, (int)KSIndex.Right);
                }
            }


            if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.HallFoward <= HallFowardPressValue && IsHallFowardPress == false) {
                IsHallFowardPress = true;

                if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.LHallForward, (int)KSKeyState.DOWN, (int)KSIndex.Left);
                } else if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.RHallForward, (int)KSKeyState.DOWN, (int)KSIndex.Right);
                }

            } else if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.inputDataKS.HallFoward > HallFowardPressValue + Meansurebias && IsHallFowardPress == true) {
                IsHallFowardPress = false;

                if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.LHallForward, (int)KSKeyState.UP, (int)KSIndex.Left);
                } else if(inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    InputDeviceKS.KeyEvent((int)KSKeyCode.RHallForward, (int)KSKeyState.UP, (int)KSIndex.Right);
                }
            }

        }

        protected virtual void ProcessKeyList() {

            if(InputDataGC.GCData.GCKeyList.Count > 0) {

                //DebugMy.Log(inputDataGetKS.inputDeviceKSPart.PartType+" ProcessKeyList:" + InputDataKS.GCData.GCKeyList.Count, this);
                //foreach(var keydata in InputDataKS.GCData.GCKeyList) {
                //    DebugMy.Log(inputDataGetKS.inputDeviceKSPart.PartType + " ProcessKeyList: deviceID:" + keydata.deivceID + "   " + keydata.keycode + " " + keydata.keyevent, this);
                //}
                InputKeyCode inputKeyCode;
                InputKeyState inputKeyState;

                if((inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft && InputDataGC.GCData.GCKeyList[0].deivceID == 0)
                    ||
                   (inputDataGetKSKey.inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight && InputDataGC.GCData.GCKeyList[0].deivceID == 1)) {

                    switch((KSKeyCode)InputDataGC.GCData.GCKeyList[0].keycode) {

                        case KSKeyCode.X:
                            inputKeyCode = InputKeyCode.X;
                            break;
                        case KSKeyCode.Y:
                            inputKeyCode = InputKeyCode.Y;
                            break;
                        case KSKeyCode.RFunction:
                            inputKeyCode = InputKeyCode.RFunction;
                            break;
                        case KSKeyCode.RjoystickKey:
                            inputKeyCode = InputKeyCode.RjoystickKey;
                            break;
                        case KSKeyCode.RHallInside:
                            inputKeyCode = InputKeyCode.RHallInside;
                            break;
                        case KSKeyCode.RHallForward:
                            inputKeyCode = InputKeyCode.RHallForward;
                            break;


                        case KSKeyCode.A:
                            inputKeyCode = InputKeyCode.A;
                            break;
                        case KSKeyCode.B:
                            inputKeyCode = InputKeyCode.B;
                            break;
                        case KSKeyCode.LFunction:
                            inputKeyCode = InputKeyCode.LFunction;
                            break;
                        case KSKeyCode.LjoystickKey:
                            inputKeyCode = InputKeyCode.LjoystickKey;
                            break;
                        case KSKeyCode.LHallInside:
                            inputKeyCode = InputKeyCode.LHallInside;
                            break;
                        case KSKeyCode.LHallForward:
                            inputKeyCode = InputKeyCode.LHallForward;
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
