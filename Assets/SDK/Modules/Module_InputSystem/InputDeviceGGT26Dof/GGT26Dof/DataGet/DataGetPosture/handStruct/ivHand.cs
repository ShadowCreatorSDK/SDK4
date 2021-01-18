using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class ivHand {
        // left = 1, right = 2;
        public static readonly int mLeft = 1;
        public static readonly int mRight = 2;

        protected static readonly int jointCount = Enum.GetNames(typeof(HandJoint)).Length;

        protected readonly Dictionary<HandJoint, IvJointPos> jointLeftPoses = new Dictionary<HandJoint, IvJointPos>();

        protected readonly Dictionary<HandJoint, IvJointPos> jointRightPoses = new Dictionary<HandJoint, IvJointPos>();

        private bool TryGetJoint(int handedness, HandJoint joint, out IvJointPos pose) {
            if(handedness == ivHand.mLeft) {
                return jointLeftPoses.TryGetValue(joint, out pose);
            } else {
                return jointRightPoses.TryGetValue(joint, out pose);
            }
        }

        public Vector3 GetJointPosition(int handedness, HandJoint jointToGet) {
            if(TryGetJoint(handedness, jointToGet, out IvJointPos pose)) {
                return pose.Position;
            }
            return Vector3.zero;
        }

        public void UpdateState(int handedness, float[] handModel) {
            for(int i = 0; i < jointCount; i++) {
                HandJoint handJoint = (HandJoint)i;
                if(handedness == ivHand.mLeft) {
                    if(!jointLeftPoses.ContainsKey(handJoint)) {
                        jointLeftPoses.Add(handJoint, new IvJointPos(getIvLeftJointPosition(i, handModel)));
                    } else {
                        jointLeftPoses[handJoint].Position = getIvLeftJointPosition(i, handModel);
                    }
                } else {
                    if(!jointRightPoses.ContainsKey(handJoint)) {
                        jointRightPoses.Add(handJoint, new IvJointPos(getIvRightJointPosition(i, handModel)));
                    } else {
                        jointRightPoses[handJoint].Position = getIvRightJointPosition(i, handModel);
                    }
                }
            }
        }

        private Vector3 getIvLeftJointPosition(int joint, float[] handModel) {

            int pos;
            switch(joint) {
                // 手腕：根据中指计算
                case 0:
                    Vector3 MiddleMetacarpal = (new Vector3(handModel[12], handModel[13], handModel[14]) + new Vector3(handModel[63], handModel[64], handModel[65])) / 2;
                    Vector3 MiddleKnuckle = new Vector3(handModel[36], handModel[37], handModel[38]);
                    Vector3 forword = MiddleMetacarpal - MiddleKnuckle;
                    return MiddleMetacarpal + forword / 3.5f;
                case 1:
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    pos = 3 + (joint - 2) * 3;
                    return new Vector3(handModel[pos], handModel[pos + 1], handModel[pos + 2]);
                // 食指掌关节
                case 10:
                    return (new Vector3(handModel[12], handModel[13], handModel[14]) + (new Vector3(handModel[12], handModel[13], handModel[14]) + new Vector3(handModel[63], handModel[64], handModel[65])) / 2) / 2;

                case 11:
                case 12:
                case 13:
                case 14:
                    pos = 3 + (joint - 3) * 3;
                    return new Vector3(handModel[pos], handModel[pos + 1], handModel[pos + 2]);
                // 中指掌关节，用5和25求中间值
                case 15:
                    return (new Vector3(handModel[12], handModel[13], handModel[14]) + new Vector3(handModel[63], handModel[64], handModel[65])) / 2;

                case 16:
                case 17:
                case 18:
                case 19:
                    pos = 3 + (joint - 4) * 3;
                    return new Vector3(handModel[pos], handModel[pos + 1], handModel[pos + 2]);
                // 无名指掌关节
                case 20:
                    return (new Vector3(handModel[63], handModel[64], handModel[65]) + (new Vector3(handModel[12], handModel[13], handModel[14]) + new Vector3(handModel[63], handModel[64], handModel[65])) / 2) / 2;

                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    pos = 3 + (joint - 5) * 3;
                    return new Vector3(handModel[pos], handModel[pos + 1], handModel[pos + 2]);
            }
            return Vector3.zero;
        }


        private Vector3 getIvRightJointPosition(int joint, float[] handModel) {

            if (handModel[0] == 1) {
                return getIvLeftJointPosition(joint, handModel);
            }

            int index = 65;
            int pos;
            switch(joint) {
                // 手腕：根据中指计算
                case 0:
                    Vector3 MiddleMetacarpal = (new Vector3(handModel[12 + index], handModel[13 + index], handModel[14 + index]) + new Vector3(handModel[63 + index], handModel[64 + index], handModel[65 + index])) / 2;
                    Vector3 MiddleKnuckle = new Vector3(handModel[36 + index], handModel[37 + index], handModel[38 + index]);
                    Vector3 forword = MiddleMetacarpal - MiddleKnuckle;
                    return MiddleMetacarpal + forword / 4.5f;
                    // return (new Vector3(handModel[12 + index], handModel[13 + index], handModel[14 + index]) + new Vector3(handModel[63 + index], handModel[64 + index], handModel[65 + index])) / 2;
                case 1:
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    pos = 3 + (joint - 2) * 3;
                    return new Vector3(handModel[pos + index], handModel[pos + 1 + index], handModel[pos + 2 + index]);
                // 食指掌关节
                case 10:
                    return (new Vector3(handModel[12 + index], handModel[13 + index], handModel[14 + index]) + (new Vector3(handModel[12 + index], handModel[13 + index], handModel[14 + index]) + new Vector3(handModel[63 + index], handModel[64 + index], handModel[65 + index])) / 2) / 2;

                case 11:
                case 12:
                case 13:
                case 14:
                    pos = 3 + (joint - 3) * 3;
                    return new Vector3(handModel[pos + index], handModel[pos + 1 + index], handModel[pos + 2 + index]);
                // 中指掌关节，用5和25求中间值
                case 15:
                    return (new Vector3(handModel[12 + index], handModel[13 + index], handModel[14 + index]) + new Vector3(handModel[63 + index], handModel[64 + index], handModel[65 + index])) / 2;

                case 16:
                case 17:
                case 18:
                case 19:
                    pos = 3 + (joint - 4) * 3;
                    return new Vector3(handModel[pos + index], handModel[pos + 1 + index], handModel[pos + 2 + index]);
                // 无名指掌关节
                case 20:
                    return (new Vector3(handModel[63 + index], handModel[64 + index], handModel[65 + index]) + (new Vector3(handModel[12 + index], handModel[13 + index], handModel[14 + index]) + new Vector3(handModel[63 + index], handModel[64 + index], handModel[65 + index])) / 2) / 2;

                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    pos = 3 + (joint - 5) * 3;
                    return new Vector3(handModel[pos + index], handModel[pos + 1 + index], handModel[pos + 2 + index]);
            }
            return Vector3.zero;
        }
    }
}