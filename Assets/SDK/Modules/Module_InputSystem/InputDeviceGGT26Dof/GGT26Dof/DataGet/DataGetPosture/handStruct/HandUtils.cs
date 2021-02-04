using System;
using System.Linq;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof
{
    public class SimulatedHandUtils
    {
        private static float[] thumbAngle = new float[3]
        {
            160f, 150f, 160f
        };

        private static float thumbRangeMin = -0.5f;
        private static float thumbRangeMax = 1f;

        private static float thumbAngleMin = 76f;
        private static float thumbAngleMax = 160f;
        static int[] jointsPerFinger = { 4, 5, 5, 5, 5 };// thumb, index, middle, right, pinky

        static int[] jointsPerFingerSum;

        public static void CalculateJointRotations(int handedness, Vector3[] jointPositions, Quaternion[] jointOrientationsOut)
        {
            const int numFingers = 5;

            if (jointsPerFingerSum == null) {
                jointsPerFingerSum = new int[numFingers];
                for (int fingerIndex = 0; fingerIndex < numFingers; fingerIndex++) {
                    jointsPerFingerSum[fingerIndex] = jointsPerFinger.Take(fingerIndex).Sum();
                }
            }

            for (int fingerIndex = 0; fingerIndex < numFingers; fingerIndex++)
            {
                int jointsCurrentFinger = jointsPerFinger[fingerIndex];
                int lowIndex = (int)HandJoint.ThumbTip + jointsPerFingerSum[fingerIndex];
                int highIndex = lowIndex + jointsCurrentFinger - 1;

                // for (int jointStartidx = lowIndex; jointStartidx <= highIndex; jointStartidx++)
                for (int jointStartidx = highIndex; jointStartidx > lowIndex; jointStartidx--)
                {
                    int jointEndidx = jointStartidx - 1;//jointStartidx == lowIndex ? (int)HandJoint.Wrist : jointStartidx - 1;
                    Vector3 boneForward = (jointPositions[jointEndidx] - jointPositions[jointStartidx]);
                    Vector3 boneUp = Vector3.Cross(boneForward, GetPalmRightVector(handedness, jointPositions));
                    if (boneForward.magnitude > float.Epsilon && boneUp.magnitude > float.Epsilon)
                    {
                        Quaternion jointRotation = Quaternion.LookRotation(boneForward, boneUp);
                        //pinky
                        if (fingerIndex == 4)
                        {
                            Vector3 bonePinkyUp = Vector3.Cross(boneForward, GetPinkyRightVector(handedness, jointPositions));
                            jointRotation = Quaternion.LookRotation(boneForward, bonePinkyUp);
                        }

                        //ring
                        if (fingerIndex == 3)
                        {
                            Vector3 bongRingUp = Vector3.Cross(boneForward, GetRingRightVector(handedness, jointPositions));
                            jointRotation = Quaternion.LookRotation(boneForward, bongRingUp);
                        }

                        //middle
                        if (fingerIndex == 2)
                        {
                            Vector3 middleUp = Vector3.Cross(boneForward, GetMiddleRightVector(handedness, jointPositions));
                            jointRotation = Quaternion.LookRotation(boneForward, middleUp);
                        }

                        //index
                        if (fingerIndex == 1)
                        {
                            Vector3 indexUp = Vector3.Cross(boneForward, GetIndexRightVector(handedness, jointPositions));
                            jointRotation = Quaternion.LookRotation(boneForward, indexUp);

                        }

                        //thumb
                        if (fingerIndex == 0)
                        {
                            Vector3 thumbUpAfter = Vector3.Cross(boneForward, GetThumbRightVector(handedness, jointPositions));

                            jointRotation = Quaternion.LookRotation(boneForward, thumbUpAfter);
                        }
                        jointOrientationsOut[jointStartidx] = jointRotation;
                    }
                    else
                    {
                        jointOrientationsOut[jointStartidx] = Quaternion.identity;
                    }
                }
            }
            jointOrientationsOut[(int)HandJoint.Palm] = Quaternion.LookRotation(GetPalmForwardVector(jointPositions), GetPalmUpVector(handedness, jointPositions));
        }

        public static Vector3 GetPalmForwardVector(Vector3[] jointPositions)
        {
            Vector3 indexBase = jointPositions[(int)HandJoint.IndexKnuckle];
            Vector3 thumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];

            Vector3 thumbMetaCarpalToIndex = indexBase - thumbMetaCarpal;
            return thumbMetaCarpalToIndex.normalized;
        }

        public static Vector3 GetPalmUpVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 indexBase = jointPositions[(int)HandJoint.IndexKnuckle];
            Vector3 pinkyBase = jointPositions[(int)HandJoint.PinkyKnuckle];
            Vector3 ThumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];

            Vector3 ThumbMetaCarpalToPinky = pinkyBase - ThumbMetaCarpal;
            Vector3 ThumbMetaCarpalToIndex = indexBase - ThumbMetaCarpal;
            if (handedness == ivHand.mLeft)
            {
                return Vector3.Cross(ThumbMetaCarpalToPinky, ThumbMetaCarpalToIndex).normalized;
            }
            else
            {
                return Vector3.Cross(ThumbMetaCarpalToIndex, ThumbMetaCarpalToPinky).normalized;
            }
        }


        public static Vector3 GetPalmRightVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 indexBase = jointPositions[(int)HandJoint.IndexKnuckle];
            Vector3 pinkyBase = jointPositions[(int)HandJoint.PinkyKnuckle];
            Vector3 thumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];

            Vector3 thumbMetaCarpalToPinky = pinkyBase - thumbMetaCarpal;
            Vector3 thumbMetaCarpalToIndex = indexBase - thumbMetaCarpal;
            Vector3 thumbMetaCarpalUp = Vector3.zero;
            if (handedness == ivHand.mLeft)
            {
                thumbMetaCarpalUp = Vector3.Cross(thumbMetaCarpalToPinky, thumbMetaCarpalToIndex).normalized;
            }
            else
            {
                thumbMetaCarpalUp = Vector3.Cross(thumbMetaCarpalToIndex, thumbMetaCarpalToPinky).normalized;
            }

            return Vector3.Cross(thumbMetaCarpalUp, thumbMetaCarpalToIndex).normalized;
        }

        private static Vector3 GetPinkyRightVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 thumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];
            Vector3 pinkyBase = jointPositions[(int)HandJoint.PinkyKnuckle];

            Vector3 palmUp = GetPalmUpVector(handedness, jointPositions);
            Vector3 pinkyDir = pinkyBase - thumbMetaCarpal;

            return Vector3.Cross(palmUp, pinkyDir).normalized;
        }

        private static Vector3 GetRingRightVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 thumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];
            Vector3 ringBase = jointPositions[(int)HandJoint.RingKnuckle];

            Vector3 palmUp = GetPalmUpVector(handedness, jointPositions);
            Vector3 ringDir = ringBase - thumbMetaCarpal;

            return Vector3.Cross(palmUp, ringDir).normalized;
        }

        private static Vector3 GetMiddleRightVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 thumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];
            Vector3 middleBase = jointPositions[(int)HandJoint.MiddleKnuckle];

            Vector3 palmUp = GetPalmUpVector(handedness, jointPositions);
            Vector3 middleDir = middleBase - thumbMetaCarpal;

            return Vector3.Cross(palmUp, middleDir).normalized;
        }

        private static Vector3 GetIndexRightVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 thumbMetaCarpal = jointPositions[(int)HandJoint.ThumbMetacarpalJoint];
            Vector3 indexBase = jointPositions[(int)HandJoint.IndexKnuckle];

            Vector3 palmUp = GetPalmUpVector(handedness, jointPositions);
            Vector3 indexDir = indexBase - thumbMetaCarpal;

            return Vector3.Cross(palmUp, indexDir).normalized;
        }

        private static Vector3 GetThumbRightVector(int handedness, Vector3[] jointPositions)
        {
            Vector3 palmUp = GetPalmUpVector(handedness, jointPositions);
            Vector3 wrist = jointPositions[(int)HandJoint.ThumbDistalJoint];
            Vector3 middleMcp = jointPositions[(int)HandJoint.MiddleKnuckle];
            Vector3 thumbRight = ((middleMcp - wrist).normalized + palmUp.normalized).normalized;
            if (handedness == ivHand.mLeft)
            {
                thumbRight = -thumbRight;
            }
            return thumbRight;
        }
    }
}
