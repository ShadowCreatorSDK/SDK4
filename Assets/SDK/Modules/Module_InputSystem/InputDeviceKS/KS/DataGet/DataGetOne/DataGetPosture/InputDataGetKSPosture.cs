using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDataGetKSPosture : InputDataGetGCPosture {
        public InputDataGetKS inputDataGetKS;

        public InputDataGetKSPosture(InputDataGetKS _inputDataGetKS) : base(_inputDataGetKS) {
            inputDataGetKS = _inputDataGetKS;
        }

        public Vector3 rightPositionDeltaWithHead = new Vector3(0.15f, -0.25f, 0.25f);
        public Vector3 leftPositionDeltaWithHead = new Vector3(-0.15f, -0.25f, 0.25f);

        private static readonly Matrix4x4 FLIP_Z = Matrix4x4.Scale(new Vector3(1, 1, -1));
        private Matrix4x4 mPoseMatrix1;
        float[] array = new float[16];

        public float PositionLerp = 1f;
        public float RotationLerp = 1f;


        Quaternion rotation;
        Vector3 position;
        Vector3 deltaEulerAngles;
        
        public override void OnSCStart() {
            base.OnSCStart();
            Update3DofDeltaEulerAngles();
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateTransform();
        }

        void UpdateTransform() {

            if(SvrManager.Instance == null || SvrManager.Instance.IsRunning == false) {
                return;
            }

            //DebugMy.Log("UpdateTransform:" + inputDataGetKS.inputDeviceKSPart.PartType+" "+Time.frameCount, this, true);

            if(Application.platform != RuntimePlatform.Android) {
                if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                    inputDataGetKS.inputDeviceKSPart.inputDataKS.rotation = SvrManager.Instance.head.rotation;
                }

                if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                        inputDataGetKS.inputDeviceKSPart.inputDataKS.position = SvrManager.Instance.head.position + leftPositionDeltaWithHead;
                    }
                } else if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                        inputDataGetKS.inputDeviceKSPart.inputDataKS.position = SvrManager.Instance.head.position + rightPositionDeltaWithHead;
                    }
                } else {
                    inputDataGetKS.inputDeviceKSPart.inputDataKS.position = Vector3.zero;
                }

                return;
            }

            int result = 0;

            if(postureType == PostureType._6Dof) {

                if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    result = SvrPlugin.Instance.Fetch6dofHandShank(array, 0);
                } else if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    result = SvrPlugin.Instance.Fetch6dofHandShank(array, 1);
                }

                if(result != 0 || array == null)
                    return;

                rotation = new Quaternion(array[3], array[4], -array[5], array[6]);
                position = new Vector3(-array[0], -array[1], array[2]);

                inputDataGetKS.inputDeviceKSPart.inputDataKS.rotation = Quaternion.Lerp(inputDataGetKS.inputDeviceKSPart.inputDataKS.rotation, rotation, RotationLerp);
                inputDataGetKS.inputDeviceKSPart.inputDataKS.position = Vector3.Lerp(inputDataGetKS.inputDeviceKSPart.inputDataKS.position, position, PositionLerp);


            } else if(postureType == PostureType._3Dof) {
                //Position
                if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                        inputDataGetKS.inputDeviceKSPart.inputDataKS.position = SvrManager.Instance.head.position + leftPositionDeltaWithHead;
                    }
                } else if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                        inputDataGetKS.inputDeviceKSPart.inputDataKS.position = SvrManager.Instance.head.position + rightPositionDeltaWithHead;
                    }
                } else {
                    DebugMy.LogError(inputDataGetKS.inputDeviceKSPart.PartType+ " Position Error", this);
                    inputDataGetKS.inputDeviceKSPart.inputDataKS.position = Vector3.zero;
                }

                //Rotation
                if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    result = SvrPlugin.Instance.Fetch3dofHandShank(array, 0);
                } else if(inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    result = SvrPlugin.Instance.Fetch3dofHandShank(array, 1);
                }

                if(result != 0 || array == null) {
                    DebugMy.LogError(inputDataGetKS.inputDeviceKSPart.PartType+" ScFetch3dofHandShank Error", this);
                    return;
                }

                for(int index = 0; index < 16; index++) {
                    mPoseMatrix1[index] = array[index];
                }

                mPoseMatrix1 = Matrix4x4.Transpose(mPoseMatrix1);

                if(mPoseMatrix1.GetColumn(2) == Vector4.zero && mPoseMatrix1.GetColumn(1) == Vector4.zero) {
                    DebugMy.LogError(inputDataGetKS.inputDeviceKSPart.PartType+" Rotation Error", this);
                    rotation = Quaternion.identity;
                } else {
                    rotation = Quaternion.LookRotation(mPoseMatrix1.GetColumn(2), mPoseMatrix1.GetColumn(1));
                }

                EffectByCalibrationKey();
                rotation = Quaternion.Euler(deltaEulerAngles + rotation.eulerAngles);

                inputDataGetKS.inputDeviceKSPart.inputDataKS.rotation = Quaternion.Lerp(inputDataGetKS.inputDeviceKSPart.inputDataKS.rotation, rotation, RotationLerp);

            }

        }

        void EffectByCalibrationKey() {
            InputKeyState inputKeyState;
            
            inputDataGetKS.inputDeviceKSPart.inputDataKS.inputKeys.inputKeyPressDic.TryGetValue(inputDataGetKS.inputDeviceKSPart.inputDataKS.CalibrationKeyAlias, out inputKeyState);

            if(inputKeyState == InputKeyState.DOWN || inputKeyState == InputKeyState.LONG) {
                DebugMy.Log(inputDataGetKS.inputDeviceKSPart.inputDataKS.CalibrationKeyAlias + " LONG Key Reset", this,true);
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

        protected override void UpdatePosition() { 
        }

        protected override void UpdateRotation() { 
        }
    }
}
