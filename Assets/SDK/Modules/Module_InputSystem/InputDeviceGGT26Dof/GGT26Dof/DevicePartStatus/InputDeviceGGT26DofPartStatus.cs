using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {

    public class InputDeviceGGT26DofPartStatus : InputDeviceHandPartStatus {

        public InputDeviceGGT26DofPart inputDeviceGGT26DofPart;
        public InputDeviceGGT26DofPartStatus(InputDeviceGGT26DofPart _inputDeviceHandPart) : base(_inputDeviceHandPart) {
            inputDeviceGGT26DofPart = _inputDeviceHandPart;
        }

        float timer = 0;
        static bool isUpdateDataThisFrame = false;

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            ///每帧只执行一次
            if (isUpdateDataThisFrame == false) {
                isUpdateDataThisFrame = true;
                UpdateStatus(InputDataGGT26Dof.handsInfo.originDataMode, InputDataGGT26Dof.handsInfo.originDataPose);
            }

            if (inputDeviceGGT26DofPart.PartType == InputDevicePartType.HandLeft) {
                inputDeviceGGT26DofPart.inputDataGGT26Dof.isFound = InputDataGGT26Dof.handsInfo.handLeftFind;
                if (inputDeviceGGT26DofPart.inputDeviceGGT26Dof.LeftHandOpen == false) {
                    inputDeviceGGT26DofPart.inputDataGGT26Dof.isFound = false;
                }
            }
            if (inputDeviceGGT26DofPart.PartType == InputDevicePartType.HandRight) {
                inputDeviceGGT26DofPart.inputDataGGT26Dof.isFound = InputDataGGT26Dof.handsInfo.handRightFind;
                if (inputDeviceGGT26DofPart.inputDeviceGGT26Dof.RightHandOpen == false) {
                    inputDeviceGGT26DofPart.inputDataGGT26Dof.isFound = false;
                }
            }

            if (inputDeviceHandPart.inputDataHand.isFound == false) {
                timer += Time.deltaTime;
                inputDeviceHandPart.inputDataHand.handInfo.lostPercent = timer / inputDeviceHandPart.inputDataHand.handInfo.lostTimer;

                if (inputDeviceHandPart.inputDataHand.handInfo.lostPercent >= 1) {
                    inputDeviceHandPart.inputDataHand.handInfo.lostPercent = 1;
                    inputDeviceHandPart.inputDataHand.handInfo.isLost = true;
                }
                ///hand都是多少百分比后将手势值设为复位值，以触发UP事件（如果已经Down或者Drag后）
                //if(inputDeviceHandPart.inputDataHand.handInfo.lostPercent > 0.90f) {
                //    //Debug.Log("========= lost > 80 :" + inputDeviceHandPart.PartType);
                //    inputDeviceHandPart.inputDataHand.ResetHandData(inputDeviceHandPart.PartType);
                //}
            } else {

                timer = 0;
                inputDeviceHandPart.inputDataHand.handInfo.lostPercent = 0;
                inputDeviceHandPart.inputDataHand.handInfo.isLost = false;
            }

            if (inputDeviceHandPart.inputDataHand.handInfo.isLost) {
                inputDeviceHandPart.inputDataBase.isVaild = false;
                //DebugMy.Log("Hand:" + inputDeviceHandPart.PartType + "  Status: DisActive !", this);
            } else {
                inputDeviceHandPart.inputDataBase.isVaild = true;
                //DebugMy.Log("Hand:" + inputDeviceHandPart.PartType + "  Status: Active", this);
            }

        }
        public override void OnSCFuncitonWaitForEndOfFrame() {
            base.OnSCFuncitonWaitForEndOfFrame();
            isUpdateDataThisFrame = false;
        }

        /// <summary>
        /// every frame invoke once
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="pose"></param>
        /// <returns></returns>
        protected virtual void UpdateStatus(float[] mode, float[] pose) {

            ///识别到手的个数
            InputDataGGT26Dof.handsInfo.handAmount = (int)mode[0];

            ///0代表不知左右
            ///1代表左
            ///2代表右
            ///可以同时0或1或2，此时舍去一个
            ///
            if ((int)mode[0] == 0) {
                InputDataGGT26Dof.handsInfo.handLeftFind = false;
                InputDataGGT26Dof.handsInfo.handRightFind = false;
            } else if ((int)mode[0] == 1) {

                if ((int)(mode[1]) == 1) {
                    InputDataGGT26Dof.handsInfo.handLeftFind = true;
                    InputDataGGT26Dof.handsInfo.handRightFind = false;
                    InputDataGGT26Dof.handsInfo.handLeftIndex = 0;
                } else if ((int)(mode[1]) == 2) {
                    InputDataGGT26Dof.handsInfo.handLeftFind = false;
                    InputDataGGT26Dof.handsInfo.handRightFind = true;
                    InputDataGGT26Dof.handsInfo.handRighIndex = 0;
                }

            } else if ((int)mode[0] == 2) {
                if ((int)(mode[1]) == 1 && (int)(mode[3 + 63]) == 2) {
                    InputDataGGT26Dof.handsInfo.handLeftFind = true;
                    InputDataGGT26Dof.handsInfo.handRightFind = true;
                    InputDataGGT26Dof.handsInfo.handLeftIndex = 0;
                    InputDataGGT26Dof.handsInfo.handRighIndex = 1;
                }
            }

            if (Application.platform != RuntimePlatform.Android) {
                InputDataGGT26Dof.handsInfo.handLeftFind = true;
                InputDataGGT26Dof.handsInfo.handRightFind = true;
                InputDataGGT26Dof.handsInfo.handRighIndex = 0;
                InputDataGGT26Dof.handsInfo.handRighIndex = 1;

                ///按K键左手丢失
                if (Input.GetKey(KeyCode.O) == true) {
                    InputDataGGT26Dof.handsInfo.handLeftFind = false;
                }
                ///按L键右手丢失
                if (Input.GetKey(KeyCode.P) == true) {
                    InputDataGGT26Dof.handsInfo.handRightFind = false;
                }
            }


            if (InputDataGGT26Dof.handsInfo.handLeftFind) {
                InputDataGGT26Dof.handsInfo.handLeft.findFrameCount++;
            } else if (InputDataGGT26Dof.handsInfo.handLeftFind == false) {
                InputDataGGT26Dof.handsInfo.handLeft.findFrameCount = 0;
            }

            if (InputDataGGT26Dof.handsInfo.handRightFind) {
                InputDataGGT26Dof.handsInfo.handRight.findFrameCount++;
            } else if (InputDataGGT26Dof.handsInfo.handRightFind == false) {
                InputDataGGT26Dof.handsInfo.handRight.findFrameCount = 0;
            }

            if (InputDataGGT26Dof.handsInfo.handLeft.findFrameCount > InputDataGGT26Dof.handsInfo.handLeft.frameCountValid) {
                InputDataGGT26Dof.handsInfo.handLeftFind = true;
            } else if (InputDataGGT26Dof.handsInfo.handLeft.isLost) {
                InputDataGGT26Dof.handsInfo.handLeftFind = false;
            }

            if (InputDataGGT26Dof.handsInfo.handRight.findFrameCount > InputDataGGT26Dof.handsInfo.handRight.frameCountValid) {
                InputDataGGT26Dof.handsInfo.handRightFind = true;
            } else if (InputDataGGT26Dof.handsInfo.handRight.isLost) {
                InputDataGGT26Dof.handsInfo.handRightFind = false;
            }

        }
    }
}
