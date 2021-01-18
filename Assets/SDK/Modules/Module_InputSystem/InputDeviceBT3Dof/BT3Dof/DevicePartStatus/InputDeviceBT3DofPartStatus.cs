using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {


public class InputDeviceBT3DofPartStatus : InputDeviceGCPartStatus {

    public InputDeviceBT3DofPart inputDeviceBT3DofPart;
        public InputDeviceBT3DofPartStatus(InputDeviceBT3DofPart inputDeviceBT3DofPart) : base(inputDeviceBT3DofPart) {
            this.inputDeviceBT3DofPart = inputDeviceBT3DofPart;
        }

        const string K02Name = "K02";
        const string K07Name = "K07";

        public override void OnSCStart() {
            base.OnSCStart();
            if(Application.platform != RuntimePlatform.Android) {
                bool isConnected = false;
                if(inputDeviceGCPart.inputDeviceGC.SimulateInEditorMode == true) {
                    if(inputDeviceGCPart.PartType == InputDevicePartType.GCOne) {
                        isConnected = inputDeviceBT3DofPart.inputDeviceBT3Dof.OneGCActive;
                        if(isConnected) {
                            InputDataBT3Dof.StatusDataList.Add(new InputDataBT3Dof.StatusData() { isConnected = isConnected, deviceID = (int)BT3DofIndex.BT3DofOne });
                        }
                    } else if(inputDeviceGCPart.PartType == InputDevicePartType.GCTwo) {
                        isConnected = inputDeviceBT3DofPart.inputDeviceBT3Dof.TwoGCActive;
                        if(isConnected) {
                            InputDataBT3Dof.StatusDataList.Add(new InputDataBT3Dof.StatusData() { isConnected = isConnected, deviceID = (int)BT3DofIndex.BT3DofTwo });
                        }
                    }
                }
            } else if(Application.platform == RuntimePlatform.Android) {
                if(inputDeviceGCPart.PartType == InputDevicePartType.GCOne) {
                    bool isConnected = AndroidPluginBase.ObjectFunctionCall<int>(AndroidPluginBT3Dof.BT3DofManager, "isHandShankConnected", 0) != 0 ? true : false;
                    if(isConnected) {
                        InputDataBT3Dof.StatusDataList.Add(new InputDataBT3Dof.StatusData() { isConnected = isConnected, deviceID = (int)BT3DofIndex.BT3DofOne });
                    }
                } else if(inputDeviceGCPart.PartType == InputDevicePartType.GCTwo) {
                    bool isConnected = AndroidPluginBase.ObjectFunctionCall<int>(AndroidPluginBT3Dof.BT3DofManager, "isHandShankConnected", 1) != 0 ? true : false;
                    if(isConnected) {
                        InputDataBT3Dof.StatusDataList.Add(new InputDataBT3Dof.StatusData() { isConnected = isConnected, deviceID = (int)BT3DofIndex.BT3DofTwo });
                    }
                }
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(InputDataBT3Dof.StatusDataList.Count > 0) {

                if((inputDeviceGCPart.PartType == InputDevicePartType.GCOne && InputDataBT3Dof.StatusDataList[0].deviceID == (int)BT3DofIndex.BT3DofOne)
                    ||
                   (inputDeviceGCPart.PartType == InputDevicePartType.GCTwo && InputDataBT3Dof.StatusDataList[0].deviceID == (int)BT3DofIndex.BT3DofTwo)) {

                    inputDeviceGCPart.inputDataGC.isConnected = InputDataBT3Dof.StatusDataList[0].isConnected;
                    InputDataBT3Dof.StatusDataList.RemoveAt(0);
                    DebugMy.Log(inputDeviceGCPart.PartType + " StatusChange:" + inputDeviceGCPart.inputDataGC.isConnected, this, true);
                    StatusChangeCallBack();

                }
            }
        }


        protected void StatusChangeCallBack() {

            if(inputDeviceGCPart.inputDataGC.isConnected && inputDeviceGCPart.inputDataBase.isVaild == false) {
                inputDeviceGCPart.inputDataBase.isVaild = UpdateDeviceInfo(inputDeviceBT3DofPart);
            } else {
                inputDeviceGCPart.inputDataBase.isVaild = UpdateDeviceInfo(inputDeviceBT3DofPart, true);
            }
        }

        protected virtual bool UpdateDeviceInfo(InputDeviceBT3DofPart part, bool isClear = false) {

            if(isClear == true) {
                part.inputDataGC.GCType = GCType.Null;
                part.inputDataGC.GCName = "";
                part.inputDataGC.SoftVesion = -1;
                DebugMy.LogError("UpdateDeviceInfo Clear!", this);
                return false;
            }

            BT3DofIndex index = part.inputDataBT3Dof.index;

            if(index != BT3DofIndex.BT3DofOne && index != BT3DofIndex.BT3DofTwo) {
                DebugMy.LogError("UpdateDeviceInfo Error:" + index, this);
                return false;
            }

            try {

                string typeFlag = AndroidPluginBase.ObjectFunctionCall<string>(AndroidPluginBT3Dof.BT3DofManager, "getManufacturerModel", (int)index);

                if(K07Name == typeFlag) {
                    part.inputDataGC.GCType = GCType.K07;
                } else if(K02Name == typeFlag) {
                    part.inputDataGC.GCType = GCType.K02;
                } else {
                    part.inputDataGC.GCType = GCType.K02;
                }

                if(part.inputDataGC.GCType != GCType.K02 && part.inputDataGC.GCType != GCType.K07) {
                    DebugMy.LogError("UpdateDeviceInfo Error:" + part.inputDataGC.GCType, this);
                    return false;
                }

                part.inputDataGC.GCName = part.inputDataGC.GCType.ToString();
                part.inputDataGC.SoftVesion = 0;

                DebugMy.Log("UpdateDeviceInfo : "
                    + " isConnected: " + part.inputDataGC.isConnected
                    + " GCName: " + part.inputDataGC.GCName
                    + " SoftVesion: " + part.inputDataGC.SoftVesion
                    , this, true);

            } catch(Exception e) {
                Debug.Log(e);
            }

            return true;
        }


    }
}
