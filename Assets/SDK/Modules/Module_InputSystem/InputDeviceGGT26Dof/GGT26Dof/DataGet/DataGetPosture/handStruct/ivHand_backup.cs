using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class ivHand_backup {
        // left = 1, right = 2;
        public float index = 0;

        protected static readonly int jointCount = Enum.GetNames(typeof(HandJoint)).Length;

        protected readonly Dictionary<HandJoint, IvJointPos> jointPoses = new Dictionary<HandJoint, IvJointPos>();

        private bool TryGetJoint(HandJoint joint, out IvJointPos pose) {
            return jointPoses.TryGetValue(joint, out pose);
        }

        public Vector3 GetJointPosition(HandJoint jointToGet) {
            if(TryGetJoint(jointToGet, out IvJointPos pose)) {
                return pose.Position;
            }
            return Vector3.zero;
        }

        public void UpdateState(float[] handModel) {
            for(int i = 0; i < jointCount; i++) {
                HandJoint handJoint = (HandJoint)i;
                if(handModel[1] == 1.0f) {
                    if(!jointPoses.ContainsKey(handJoint)) {
                        jointPoses.Add(handJoint, new IvJointPos(getIvJointPosition(i, handModel)));
                    } else {
                        jointPoses[handJoint].Position = getIvJointPosition(i, handModel);
                    }
                }
            }
        }

        private Vector3 getIvJointPosition(int joint, float[] handModel) {
            int pos = 0;
            switch(joint) {
                case 0:
                    break;
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
                    pos = 2 + (joint - 2) * 3;
                    return new Vector3(handModel[pos], -handModel[pos + 1], -handModel[pos + 2]);
                // 食指掌关节
                case 10:
                    return (new Vector3(handModel[11], -handModel[12], -handModel[13]) + (new Vector3(handModel[11], -handModel[12], -handModel[13]) + new Vector3(handModel[62], -handModel[63], -handModel[64])) / 2) / 2;

                case 11:
                case 12:
                case 13:
                case 14:
                    pos = 2 + (joint - 3) * 3;
                    return new Vector3(handModel[pos], -handModel[pos + 1], -handModel[pos + 2]);
                // 中指掌关节，用5和25求中间值
                case 15:
                    return (new Vector3(handModel[11], -handModel[12], -handModel[13]) + new Vector3(handModel[62], -handModel[63], -handModel[64])) / 2;

                case 16:
                case 17:
                case 18:
                case 19:
                    pos = 2 + (joint - 4) * 3;
                    return new Vector3(handModel[pos], -handModel[pos + 1], -handModel[pos + 2]);
                // 无名指掌关节
                case 20:
                    return (new Vector3(handModel[62], -handModel[63], -handModel[64]) + (new Vector3(handModel[11], -handModel[12], -handModel[13]) + new Vector3(handModel[62], -handModel[63], -handModel[64])) / 2) / 2;

                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    pos = 2 + (joint - 5) * 3;
                    return new Vector3(handModel[pos], -handModel[pos + 1], -handModel[pos + 2]);
            }
            return Vector3.zero;
        }
    }
}