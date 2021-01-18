using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand
{
    public class CubeHandModel : AbstractHandModel {
        public override HandModelType handModelType => HandModelType.CubeHand;

        public override void UpdateTransform() {
            UpdateJointTransform();
        }

        public override Transform GetJointTransform(FINGER finger, JOINT joint) {
            return fingerUI[(int)finger].jointGameObject[(int)joint].transform;
        }

        protected virtual void UpdateJointTransform() {

            for(int jointIdx = 0; jointIdx <= 3; ++jointIdx) {
                fingerUI[(int)FINGER.thumb].jointGameObject[jointIdx].transform.localPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.thumb].joint[jointIdx].localPosition;
                fingerUI[(int)FINGER.thumb].jointGameObject[jointIdx].transform.localRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.thumb].joint[jointIdx].localRotation;
            }

            for(int jointIdx = 0; jointIdx <= 3; ++jointIdx) {
                fingerUI[(int)FINGER.forefinger].jointGameObject[jointIdx].transform.localPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.forefinger].joint[jointIdx].localPosition;
                fingerUI[(int)FINGER.forefinger].jointGameObject[jointIdx].transform.localRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.forefinger].joint[jointIdx].localRotation;
            }

            for(int jointIdx = 0; jointIdx <= 3; ++jointIdx) {
                fingerUI[(int)FINGER.middle].jointGameObject[jointIdx].transform.localPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.middle].joint[jointIdx].localPosition;
                fingerUI[(int)FINGER.middle].jointGameObject[jointIdx].transform.localRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.middle].joint[jointIdx].localRotation;
            }

            for(int jointIdx = 0; jointIdx <= 3; ++jointIdx) {
                fingerUI[(int)FINGER.ring].jointGameObject[jointIdx].transform.localPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.ring].joint[jointIdx].localPosition;
                fingerUI[(int)FINGER.ring].jointGameObject[jointIdx].transform.localRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.ring].joint[jointIdx].localRotation;
            }

            for(int jointIdx = 0; jointIdx <= 4; ++jointIdx) {
                fingerUI[(int)FINGER.small].jointGameObject[jointIdx].transform.localPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.small].joint[jointIdx].localPosition;
                fingerUI[(int)FINGER.small].jointGameObject[jointIdx].transform.localRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.small].joint[jointIdx].localRotation;
            }
        }


    }
}