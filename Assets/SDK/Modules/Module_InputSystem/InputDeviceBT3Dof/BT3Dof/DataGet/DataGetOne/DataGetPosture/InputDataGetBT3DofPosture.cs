using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDataGetBT3DofPosture : InputDataGetGCPosture {
        public InputDataGetBT3Dof inputDataGetBT3Dof;

        public InputDataGetBT3DofPosture(InputDataGetBT3Dof _inputDataGetBT3Dof) : base(_inputDataGetBT3Dof) {
            inputDataGetBT3Dof = _inputDataGetBT3Dof;
        }


        public Vector3 device1PositionDeltaWithHead = new Vector3(0.15f, -0.35f, 0);
        public Vector3 device2PositionDeltaWithHead = new Vector3(-0.15f, -0.35f, 0);

        private static readonly Matrix4x4 FLIP_Z = Matrix4x4.Scale(new Vector3(1, 1, -1));
        private Matrix4x4 mPoseMatrix1;
        Quaternion rotation;
        float[] array;

        Vector3 deltaEulerAngles;
        public override void OnSCStart() {
            base.OnSCStart(); 
            
            Update3DofDeltaEulerAngles();
        }


        protected override void UpdatePosition() {

            if(Application.platform != RuntimePlatform.Android) {
                device1PositionDeltaWithHead = new Vector3(0.15f, -0.05f, 0);
                device2PositionDeltaWithHead = new Vector3(-0.15f, -0.05f, 0);
            }

            if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCOne) {
                if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                    inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.position = SvrManager.Instance.head.position + device1PositionDeltaWithHead;
                }
            } else if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCTwo) {
                if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                    inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.position = SvrManager.Instance.head.position + device2PositionDeltaWithHead;
                }
            } else {
                inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.position = Vector3.zero;
            }
        }


        protected override void UpdateRotation() {
            if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCOne) {
                array = AndroidPluginBase.ObjectFunctionCall<float[]>(AndroidPluginBT3Dof.BT3DofManager, "get3DofMatrix", 0);
            } else if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCTwo) {
                array = AndroidPluginBase.ObjectFunctionCall<float[]>(AndroidPluginBT3Dof.BT3DofManager, "get3DofMatrix", 1);
            } else {
                return;
            }
            if(Application.platform != RuntimePlatform.Android) {
                if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                    inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.rotation = SvrManager.Instance.head.rotation;
                }
                return;
            }

            if(array == null) {
                return;
            } else {
                for(int index = 0; index < 16; index++) {
                    mPoseMatrix1[index] = array[index];
                }
                mPoseMatrix1 = FLIP_Z * mPoseMatrix1.inverse * FLIP_Z;
                rotation = Quaternion.LookRotation(mPoseMatrix1.GetColumn(2), mPoseMatrix1.GetColumn(1));
            }

            EffectByCalibrationKey();
            rotation = Quaternion.Euler(deltaEulerAngles + rotation.eulerAngles);

            inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.rotation = Quaternion.Lerp(inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.rotation, rotation,0.4f);

        }

        void EffectByCalibrationKey() {
            InputKeyState inputKeyState;

            inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.inputKeys.inputKeyPressDic.TryGetValue(inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.CalibrationKeyAlias, out inputKeyState);

            if(inputKeyState == InputKeyState.LONG) {
                DebugMy.Log(inputDataGetBT3Dof.inputDeviceBT3DofPart.inputDataBT3Dof.CalibrationKeyAlias+ " LONG Key Reset", this,true);
                Update3DofDeltaEulerAngles();
            }
            //DebugMy.Log("OnUpdateRotation: " + inputDeviceHandShankPart.inputDataHandShank.rotation.eulerAngles, this);
        }

        void Update3DofDeltaEulerAngles() {
            if(SvrManager.Instance && SvrManager.Instance.gameObject.activeSelf) {
                deltaEulerAngles = SvrManager.Instance.head.transform.eulerAngles - rotation.eulerAngles;
                deltaEulerAngles.x = 0;
            }
        }
    }
}
