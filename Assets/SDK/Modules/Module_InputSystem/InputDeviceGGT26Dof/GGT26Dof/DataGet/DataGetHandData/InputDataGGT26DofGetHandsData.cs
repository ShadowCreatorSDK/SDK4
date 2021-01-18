
using UnityEngine;

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class InputDataGGT26DofGetHandsData : InputDataGetHandsData {

        public InputDataGetGGT26Dof inputDataGetGGT26Dof;
        public InputDataGGT26DofGetHandsData(InputDataGetGGT26Dof _inputDataGetGGT26Dof) : base(_inputDataGetGGT26Dof) {
            inputDataGetGGT26Dof = _inputDataGetGGT26Dof;
        }

        private float mLerpSpeed = 1f;
        public float LerpSpeed {
            get {
                return Mathf.Clamp01(mLerpSpeed)-0.001f;
            }
            set {
                mLerpSpeed = value;
            }
        }
        Vector3[] v3Pool=new Vector3[26];


        public override void OnSCAwake() {
            base.OnSCAwake();
            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "GGT26DofHandDataLerp")) {
                try {
                    LerpSpeed = API_Module_SDKConfiguration.GetFloat("Module_InputSystem", "GGT26DofHandDataLerp", 0);
                    DebugMy.Log("Lerp From SDKConfig:" + LerpSpeed, this, true);
                } catch (Exception e) {
                    Debug.Log(e);
                }
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            OnUpdateInputDataAndStore();
        }
        //static bool isHandTrackStart = false;
        static bool isUpdateDataThisFrame = false;
        void OnUpdateInputDataAndStore() {

            ///每帧只执行一次
            if(isUpdateDataThisFrame == false) {
                isUpdateDataThisFrame = true;
                InputDataAddHandData(InputDataGGT26Dof.handsInfo.originDataMode, InputDataGGT26Dof.handsInfo.originDataPose);
            }
        }

        public override void OnSCFuncitonWaitForEndOfFrame() {
            base.OnSCFuncitonWaitForEndOfFrame();
            isUpdateDataThisFrame = false;
        }

        //#else
        static Vector3 temp1, temp2;


        /// <summary>
        /// every frame invoke once
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="pose"></param>
        /// <returns></returns>
        public int InputDataAddHandData(float[] mode, float[] pose) {
            
            //DebugMy.Log("hand Amount:" + InputDataGGT26Dof.handsInfo.handAmount+"      mode[0]:" + (int)(mode[1])+" mode[65]:"+ (int)(mode[2 + 63]), this);

            if(InputDataGGT26Dof.handsInfo.handLeftFind && InputDataGGT26Dof.handsInfo.handLeft != null) {
                FillBuffer(0,mode, pose, InputDataGGT26Dof.handsInfo.handLeftIndex, InputDataGGT26Dof.handsInfo.handLeft);
            }
            if(InputDataGGT26Dof.handsInfo.handRightFind && InputDataGGT26Dof.handsInfo.handRight != null) {
                FillBuffer(1,mode, pose, InputDataGGT26Dof.handsInfo.handRighIndex, InputDataGGT26Dof.handsInfo.handRight);
            }

            return InputDataGGT26Dof.handsInfo.handAmount;
        }


        /// <summary>
        /// 填充data buffer
        /// </summary>
        /// <param name="mode"> 底层获取的原始数据 </param>
        /// <param name="pose"> 底层获取的原始数据 </param>
        /// <param name="handIdx"> 将底层数据中第index个手势数据填充到handbuf</param>
        /// <param name="handbuf"></param>
        void FillBuffer(int handIndex ,float[] mode, float[] pose, int MemoryIdx, handInfo handbuf) {
            if(Application.platform != RuntimePlatform.Android) {
                handbuf.localPosition.x = pose[0];
                handbuf.localPosition.y = pose[1];
                handbuf.localPosition.z = pose[2];

                ///按J键右手向左
                if(Input.GetKey(KeyCode.J) == true) {
                    temp1 += Vector3.back * Time.deltaTime * 0.1f;
                }
                ///按L键右手向右
                if(Input.GetKey(KeyCode.L) == true) {
                    temp1 += Vector3.back * Time.deltaTime * -0.1f;
                }
                ///按K键右手向后
                if(Input.GetKey(KeyCode.K) == true) {
                    temp2 += Vector3.back * Time.deltaTime * 0.1f;
                }
                ///按I键右手向前
                if(Input.GetKey(KeyCode.I) == true) {
                    temp2 += Vector3.back * Time.deltaTime * -0.1f;
                }

                if(Input.GetKey(KeyCode.Escape)) {
                    temp2 = temp1 = Vector3.zero;
                }

                if(MemoryIdx == 1) {
                    handbuf.localPosition.x = temp1.z;
                    handbuf.localPosition.z = temp2.z;
                }
            } else {
                handbuf.localPosition.x = 0;
                handbuf.localPosition.y = 0;
                handbuf.localPosition.z = 0;

            }

            // left hand
            if(handIndex == 0) {

                leftHand.UpdateState(ivHand.mLeft, mode);
                for(int i = 0; i < jointCount; i++) {
                    jointPositions[i] = leftHand.GetJointPosition(ivHand.mLeft, (HandJoint)i);
                }
                SimulatedHandUtils.CalculateJointRotations(ivHand.mLeft, jointPositions, jointOrientationsOut);

                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localRotation, jointOrientationsOut[(int)HandJoint.ThumbMetacarpalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localRotation, jointOrientationsOut[(int)HandJoint.ThumbProximalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localRotation, jointOrientationsOut[(int)HandJoint.ThumbDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localRotation, jointOrientationsOut[(int)HandJoint.ThumbTip], LerpSpeed);

                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localRotation, jointOrientationsOut[(int)HandJoint.IndexKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localRotation, jointOrientationsOut[(int)HandJoint.IndexMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localRotation, jointOrientationsOut[(int)HandJoint.IndexDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localRotation, jointOrientationsOut[(int)HandJoint.IndexTip], LerpSpeed);

                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localRotation, jointOrientationsOut[(int)HandJoint.MiddleKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localRotation, jointOrientationsOut[(int)HandJoint.MiddleMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localRotation, jointOrientationsOut[(int)HandJoint.MiddleDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.One].localRotation, jointOrientationsOut[(int)HandJoint.MiddleTip], LerpSpeed);

                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localRotation, jointOrientationsOut[(int)HandJoint.RingKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localRotation, jointOrientationsOut[(int)HandJoint.RingMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localRotation, jointOrientationsOut[(int)HandJoint.RingDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.One].localRotation, jointOrientationsOut[(int)HandJoint.RingTip], LerpSpeed);

                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Five].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Five].localRotation, jointOrientationsOut[(int)HandJoint.PinkyMetacarpal], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Four].localRotation, jointOrientationsOut[(int)HandJoint.PinkyKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Three].localRotation, jointOrientationsOut[(int)HandJoint.PinkyMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Two].localRotation, jointOrientationsOut[(int)HandJoint.PinkyDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.One].localRotation, jointOrientationsOut[(int)HandJoint.PinkyTip], LerpSpeed);

            } else if(handIndex == 1) {///right hand

                rightHand.UpdateState(ivHand.mRight, mode);
                for(int i = 0; i < jointCount; i++) {
                    jointPositionsRight[i] = rightHand.GetJointPosition(ivHand.mRight, (HandJoint)i);
                }
                SimulatedHandUtils.CalculateJointRotations(ivHand.mRight, jointPositionsRight, jointOrientationsOutRight);

                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localRotation, jointOrientationsOutRight[(int)HandJoint.ThumbMetacarpalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localRotation, jointOrientationsOutRight[(int)HandJoint.ThumbProximalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localRotation, jointOrientationsOutRight[(int)HandJoint.ThumbDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localRotation, jointOrientationsOutRight[(int)HandJoint.ThumbTip], LerpSpeed);

                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localRotation, jointOrientationsOutRight[(int)HandJoint.IndexKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localRotation, jointOrientationsOutRight[(int)HandJoint.IndexMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localRotation, jointOrientationsOutRight[(int)HandJoint.IndexDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localRotation, jointOrientationsOutRight[(int)HandJoint.IndexTip], LerpSpeed);

                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localRotation, jointOrientationsOutRight[(int)HandJoint.MiddleKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localRotation, jointOrientationsOutRight[(int)HandJoint.MiddleMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localRotation, jointOrientationsOutRight[(int)HandJoint.MiddleDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.middle].joint[(int)JOINT.One].localRotation, jointOrientationsOutRight[(int)HandJoint.MiddleTip], LerpSpeed);

                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localRotation, jointOrientationsOutRight[(int)HandJoint.RingKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localRotation, jointOrientationsOutRight[(int)HandJoint.RingMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localRotation, jointOrientationsOutRight[(int)HandJoint.RingDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.ring].joint[(int)JOINT.One].localRotation, jointOrientationsOutRight[(int)HandJoint.RingTip], LerpSpeed);

                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Five].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Five].localRotation, jointOrientationsOutRight[(int)HandJoint.PinkyMetacarpal], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Four].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Four].localRotation, jointOrientationsOutRight[(int)HandJoint.PinkyKnuckle], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Three].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Three].localRotation, jointOrientationsOutRight[(int)HandJoint.PinkyMiddleJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Two].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.Two].localRotation, jointOrientationsOutRight[(int)HandJoint.PinkyDistalJoint], LerpSpeed);
                handbuf.finger[(int)FINGER.small].joint[(int)JOINT.One].localRotation = Quaternion.Lerp(handbuf.finger[(int)FINGER.small].joint[(int)JOINT.One].localRotation, jointOrientationsOutRight[(int)HandJoint.PinkyTip], LerpSpeed);
            }


            //Debug.Log("wangcq327 --- handsPos:" + handsInfo.handList[handIdx].localPosition.x+":"+ handsInfo.handList[handIdx].localPosition.y+":"+ handsInfo.handList[handIdx].localPosition.z);
            for(int jointIdx = 0; jointIdx <= 3; ++jointIdx) {
                if(v3Pool[jointIdx] == null) { v3Pool[jointIdx] = new Vector3(); }

                v3Pool[jointIdx].x = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3];
                v3Pool[jointIdx].y = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 1];
                v3Pool[jointIdx].z = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 2];

                handbuf.finger[(int)FINGER.thumb].joint[jointIdx].localPosition = Vector3.Lerp(handbuf.finger[(int)FINGER.thumb].joint[jointIdx].localPosition, v3Pool[jointIdx], LerpSpeed);

                //DebugMy.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.thumb + ":" + handbuf.finger[(int)FINGER.thumb].joint[jointIdx].localPosition * 1000,this);
                //Debug.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.thumb + ":" + handbuf.finger[(int)FINGER.thumb].joint[jointIdx].localPosition * 1000);
            }
            for(int jointIdx = 4; jointIdx <= 7; ++jointIdx) {
                if(v3Pool[jointIdx] == null) { v3Pool[jointIdx] = new Vector3(); }

                v3Pool[jointIdx].x = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3];
                v3Pool[jointIdx].y = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 1];
                v3Pool[jointIdx].z = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 2];

                handbuf.finger[(int)FINGER.forefinger].joint[jointIdx - 4].localPosition = Vector3.Lerp(handbuf.finger[(int)FINGER.forefinger].joint[jointIdx - 4].localPosition, v3Pool[jointIdx], LerpSpeed);

                //DebugMy.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.forefinger + ":" + handbuf.finger[(int)FINGER.forefinger].joint[jointIdx - 4].localPosition * 1000, this);
                //Debug.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.forefinger + ":" + handbuf.finger[(int)FINGER.forefinger].joint[jointIdx - 4].localPosition * 1000);
            }
            for(int jointIdx = 8; jointIdx <= 11; ++jointIdx) {
                if(v3Pool[jointIdx] == null) { v3Pool[jointIdx] = new Vector3(); }

                v3Pool[jointIdx].x = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3];
                v3Pool[jointIdx].y = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 1];
                v3Pool[jointIdx].z = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 2];

                handbuf.finger[(int)FINGER.middle].joint[jointIdx - 8].localPosition = Vector3.Lerp(handbuf.finger[(int)FINGER.middle].joint[jointIdx - 8].localPosition, v3Pool[jointIdx], LerpSpeed);

                //DebugMy.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.middle + ":" + handbuf.finger[(int)FINGER.middle].joint[jointIdx - 8].localPosition * 1000, this);
                //Debug.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.middle + ":" + handbuf.finger[(int)FINGER.middle].joint[jointIdx - 8].localPosition * 1000);
            }
            for(int jointIdx = 12; jointIdx <= 15; ++jointIdx) {
                if(v3Pool[jointIdx] == null) { v3Pool[jointIdx] = new Vector3(); }

                v3Pool[jointIdx].x = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3];
                v3Pool[jointIdx].y = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 1];
                v3Pool[jointIdx].z = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 2];

                handbuf.finger[(int)FINGER.ring].joint[jointIdx - 12].localPosition = Vector3.Lerp(handbuf.finger[(int)FINGER.ring].joint[jointIdx - 12].localPosition, v3Pool[jointIdx], LerpSpeed);

                //DebugMy.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.ring + ":" + handbuf.finger[(int)FINGER.ring].joint[jointIdx - 12].localPosition * 1000, this);
                //Debug.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.ring + ":" + handbuf.finger[(int)FINGER.ring].joint[jointIdx - 12].localPosition * 1000);
            }
            for(int jointIdx = 16; jointIdx <= 20; ++jointIdx) {
                if(v3Pool[jointIdx] == null) { v3Pool[jointIdx] = new Vector3(); }

                v3Pool[jointIdx].x = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3];
                v3Pool[jointIdx].y = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 1];
                v3Pool[jointIdx].z = mode[3 + MemoryIdx * 2 + 63 * MemoryIdx + jointIdx * 3 + 2];

                handbuf.finger[(int)FINGER.small].joint[jointIdx - 16].localPosition = Vector3.Lerp(handbuf.finger[(int)FINGER.small].joint[jointIdx - 16].localPosition, v3Pool[jointIdx], LerpSpeed);

                //DebugMy.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.small + ":" + handbuf.finger[(int)FINGER.small].joint[jointIdx - 16].localPosition * 1000, this);
                //Debug.Log("wangcq327 --- handID:" + handIdx + ":" + FINGER.small + ":" + handbuf.finger[(int)FINGER.small].joint[jointIdx - 16].localPosition * 1000);
            }


        }

        private ivHand leftHand = new ivHand();
        private ivHand rightHand = new ivHand();
        private Quaternion[] jointOrientationsOut = new Quaternion[26];
        private Quaternion[] jointOrientationsOutRight = new Quaternion[26];
        private Vector3[] jointPositions = new Vector3[26];
        private Vector3[] jointPositionsRight = new Vector3[26];
        int jointCount = Enum.GetNames(typeof(HandJoint)).Length;
        //void OnUpdateJointRotation() {
        //    if(InputDataGGT26Dof.handsInfo.originDataMode[0] > 0) {
        //        leftHand.UpdateState(InputDataGGT26Dof.handsInfo.originDataMode);
        //        for(int i = 0; i < jointCount; i++) {
        //            jointPositions[i] = leftHand.GetJointPosition((HandJoint)i);
        //        }
        //        SimulatedHandUtils.CalculateJointRotations(ivHand.mLeft, jointPositions, jointOrientationsOut);
        //    }
        //}

    }
}