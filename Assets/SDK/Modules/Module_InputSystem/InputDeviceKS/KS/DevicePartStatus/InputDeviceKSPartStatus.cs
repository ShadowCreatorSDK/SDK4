using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {


public class InputDeviceKSPartStatus : InputDeviceGCPartStatus {

    public InputDeviceKSPart inputDeviceKSPart;
        public InputDeviceKSPartStatus(InputDeviceKSPart inputDeviceKSPart) : base(inputDeviceKSPart) {
            this.inputDeviceKSPart = inputDeviceKSPart;
        }
        bool invokeOnce = false;


        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(invokeOnce == false && SvrManager.Instance.IsRunning) {
                invokeOnce = true;
                bool isConnected = false;
                if(Application.platform != RuntimePlatform.Android) {
                    if(inputDeviceKSPart.inputDeviceKS.SimulateInEditorMode == true) {
                        if(inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                            isConnected = inputDeviceKSPart.inputDeviceKS.LeftActive;
                            if(isConnected) {
                                InputDataKS.StatusDataList.Add(new InputDataKS.StatusData() { isConnected = isConnected, deviceID = (int)KSIndex.Left });
                            }
                        } else if(inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                            isConnected = inputDeviceKSPart.inputDeviceKS.RightActive;
                            if(isConnected) {
                                InputDataKS.StatusDataList.Add(new InputDataKS.StatusData() { isConnected = isConnected, deviceID = (int)KSIndex.Right });
                            }
                        }
                    }
                } else if(Application.platform == RuntimePlatform.Android) {
                    if(inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                        isConnected = SvrPlugin.Instance.HandShank_GetConnectState(0);
                        if(isConnected) {
                            InputDataKS.StatusDataList.Add(new InputDataKS.StatusData() { isConnected = isConnected, deviceID = (int)KSIndex.Left });
                        }
                    } else if(inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                        isConnected = SvrPlugin.Instance.HandShank_GetConnectState(1);
                        if(isConnected) {
                            InputDataKS.StatusDataList.Add(new InputDataKS.StatusData() { isConnected = isConnected, deviceID = (int)KSIndex.Right });
                        }
                    }
                }
            }

            if(InputDataKS.StatusDataList.Count > 0) {

                if((inputDeviceKSPart.PartType == InputDevicePartType.KSLeft && InputDataKS.StatusDataList[0].deviceID == (int)KSIndex.Left)
                    ||
                   (inputDeviceKSPart.PartType == InputDevicePartType.KSRight && InputDataKS.StatusDataList[0].deviceID == (int)KSIndex.Right)) {

                    inputDeviceKSPart.inputDataKS.isConnected = InputDataKS.StatusDataList[0].isConnected;
                    InputDataKS.StatusDataList.RemoveAt(0);
                    DebugMy.Log(inputDeviceKSPart.PartType + " StatusChange:" + inputDeviceKSPart.inputDataKS.isConnected, this, true);
                    StatusChangeCallBack();

                }
            }
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            invokeOnce = false;
        }

        protected void StatusChangeCallBack() {

            if(inputDeviceKSPart.inputDataKS.isConnected && inputDeviceKSPart.inputDataBase.isVaild == false) {
                inputDeviceKSPart.inputDataBase.isVaild = UpdateDeviceInfo(inputDeviceKSPart);
                DebugMy.Log("Normal Connect !", this, true);
            } else if (inputDeviceKSPart.inputDataKS.isConnected && inputDeviceKSPart.inputDataBase.isVaild) {
                DebugMy.Log("Server Restart !",this,true);
            } else {
                inputDeviceKSPart.inputDataBase.isVaild = UpdateDeviceInfo(inputDeviceKSPart,true);
                DebugMy.Log("Normal DisConnect !", this, true);
            }
        }

        protected virtual bool UpdateDeviceInfo(InputDeviceKSPart part,bool isClear=false) {

            if(isClear == true) {
                part.inputDataKS.GCType = GCType.Null;
                part.inputDataKS.GCName = "";
                part.inputDataKS.SoftVesion = -1;
                part.inputDataKS.BatteryPower = -1;
                DebugMy.LogError("UpdateDeviceInfo Clear!", this);
                return false;
            }

            KSIndex index = part.inputDataKS.ksIndex;

            if(index != KSIndex.Left && index != KSIndex.Right) {
                DebugMy.LogError("UpdateDeviceInfo Error:" + index, this);
                return false;
            }

            try {

                int typeFlag = SvrPlugin.Instance.HandShank_Getbond((int)index);

                if(index == KSIndex.Left) {
                    if((int)KSTypeFlag.K11 == (typeFlag & 0xF)) {
                        part.inputDataKS.GCType = GCType.K11;
                    } else if((int)KSTypeFlag.K101 == (typeFlag & 0xF)) {
                        if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "UseK102Model")) {
                            part.inputDataKS.GCType = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "UseK102Model", 0) ? GCType.K102 : GCType.K101;
                        } else {
                            part.inputDataKS.GCType = GCType.K101;
                        }
                    }
                } else if(part.inputDataKS.ksIndex == KSIndex.Right) {
                    if((int)KSTypeFlag.K11 == ((typeFlag & 0xF0) >> 4)) {
                        part.inputDataKS.GCType = GCType.K11;
                    } else if((int)KSTypeFlag.K101 == ((typeFlag & 0xF0) >> 4)) {
                        if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "UseK102Model")) {
                            part.inputDataKS.GCType = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "UseK102Model", 0) ? GCType.K102 : GCType.K101;
                        } else {
                            part.inputDataKS.GCType = GCType.K101;
                        }
                    }
                }

                if(part.inputDataKS.GCType != GCType.K11 && part.inputDataKS.GCType != GCType.K101 && part.inputDataKS.GCType != GCType.K102) {
                    DebugMy.LogError("UpdateDeviceInfo Error:" + part.inputDataKS.GCType, this);
                    return false;
                }

                part.inputDataKS.GCName = part.inputDataKS.GCType.ToString();
                part.inputDataKS.SoftVesion = SvrPlugin.Instance.HandShank_GetVersion((int)index);
                part.inputDataKS.BatteryPower = SvrPlugin.Instance.HandShank_GetBattery((int)index);


                if (part.PostureType == PostureType.UnKown) {
                    if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "KSMode6Dof")) {
                        part.PostureType = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "KSMode6Dof", 0) ? PostureType._6Dof : PostureType._3Dof;
                    }
                }

                DebugMy.Log("UpdateDeviceInfo : "
                    + " isConnected="+part.inputDataKS.isConnected
                    + " GCName=" + part.inputDataKS.GCName
                    + " PostureType=" + part.PostureType
                    + " SoftVesion=" + part.inputDataKS.SoftVesion
                    + " BatteryPower=" + part.inputDataKS.BatteryPower
                    , this, true);

            } catch(Exception e) {
                Debug.Log(e);
            }

            return true;
        }
        

    }
}
