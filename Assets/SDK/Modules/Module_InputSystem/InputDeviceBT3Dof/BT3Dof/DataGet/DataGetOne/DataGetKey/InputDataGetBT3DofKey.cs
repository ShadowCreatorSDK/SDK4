using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDataGetBT3DofKey : InputDataGetGCKey {

        public InputDataGetBT3Dof inputDataGetBT3Dof;
        public BT3DofKeyListener BT3DofKeyListener;

        /// <summary>
        /// 确保GCKeyListener只会注册一次
        /// </summary>
        public static bool isInvokeOnce = false;

        public InputDataGetBT3DofKey(InputDataGetBT3Dof _inputDataGetBT3Dof) : base(_inputDataGetBT3Dof) {


            inputDataGetBT3Dof = _inputDataGetBT3Dof;

            if(isInvokeOnce == false) {
                isInvokeOnce = true;
                if(Application.platform == RuntimePlatform.Android) {
                    BT3DofKeyListener = new BT3DofKeyListener(this);
                    AndroidPluginBase.ObjectAddListener(AndroidPluginBT3Dof.BT3DofManager, "setHandShankKeyEventCallback", BT3DofKeyListener);

                }
            }

        }

        public override void OnUpdateKey() {
            base.OnUpdateKey();
            ProcessKeyList();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            BT3DofKeyListener = null;
            inputDataGetBT3Dof = null;
            isInvokeOnce = false;
        }

        protected virtual void ProcessKeyList() {

            if(InputDataGC.GCData.GCKeyList.Count > 0) {

                //DebugMy.Log(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType+" ProcessKeyList:" + InputDataBT3Dof.GCData.GCKeyList.Count, this);
                //foreach(var keydata in InputDataBT3Dof.GCData.GCKeyList) {
                //    DebugMy.Log(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType + " ProcessKeyList: deviceID:" + keydata.deivceID + "   " + keydata.keycode + " " + keydata.keyevent, this);
                //}
                InputKeyCode inputKeyCode;
                InputKeyState inputKeyState;

                if((inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCOne && InputDataGC.GCData.GCKeyList[0].deivceID == 0)
                    ||
                   (inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCTwo && InputDataGC.GCData.GCKeyList[0].deivceID == 1)) {

                    switch((BT3DofKeyCode)InputDataGC.GCData.GCKeyList[0].keycode) {
                        case BT3DofKeyCode.BACK:
                            inputKeyCode = InputKeyCode.Back;
                            break;
                        case BT3DofKeyCode.TIGGER:
                            inputKeyCode = InputKeyCode.Trigger;
                            break;
                        case BT3DofKeyCode.FUNCTION:
                            inputKeyCode = InputKeyCode.Function;
                            break;
                        case BT3DofKeyCode.TP:
                            inputKeyCode = InputKeyCode.Tp;
                            break;
                        case BT3DofKeyCode.VOLUMEDOWN:
                            inputKeyCode = InputKeyCode.VolumeDown;
                            break;
                        case BT3DofKeyCode.VOLUMEUP:
                            inputKeyCode = InputKeyCode.VolumeUp;
                            break;
                        default:
                            inputKeyCode = InputKeyCode.OTHER;
                            break;
                    }

                    switch((BT3DofKeyState)InputDataGC.GCData.GCKeyList[0].keyevent) {
                        case BT3DofKeyState.UP:
                            inputKeyState = InputKeyState.UP;
                            break;
                        case BT3DofKeyState.DOWN:
                            inputKeyState = InputKeyState.DOWN;
                            break;
                        case BT3DofKeyState.LONG:
                            inputKeyState = InputKeyState.LONG;
                            break;
                        default:
                            inputKeyState = InputKeyState.Null;
                            break;
                    }

                    inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.inputKeys.InputDataAddKey(inputKeyCode, inputKeyState);
                    DebugMy.Log(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType + " ProcessKeyList:" + inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType + " Add Key:" + inputKeyCode + "  State:" + inputKeyState, this);

                    ///Enter别名
                    if(inputKeyCode == inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.EnterKeyAlias) {
                        inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.inputKeys.InputDataAddKey(InputKeyCode.Enter, inputKeyState);
                        DebugMy.Log(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType + " ProcessKeyList:" + inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType + " Add (Alias:" + inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.EnterKeyAlias + ") Key:" + InputKeyCode.Enter + "  State:" + inputKeyState, this);
                    }

                    InputDataGC.GCData.GCKeyList.RemoveAt(0);

                }

            }
        }

    }
}
