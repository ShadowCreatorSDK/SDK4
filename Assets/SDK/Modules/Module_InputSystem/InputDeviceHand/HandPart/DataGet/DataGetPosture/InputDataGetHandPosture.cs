using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static SC.XR.Unity.Module_InputSystem.InputDeviceHand.ModelHand;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class InputDataGetHandPosture : InputDataGetOneBase {
        public InputDataGetHand inputDataGetHand;
        public InputDataGetHandPosture(InputDataGetHand _inputDataGetHand) : base(_inputDataGetHand) {
            inputDataGetHand = _inputDataGetHand;
        }

        protected ModelHand modelHand {
            get {
                return inputDataGetHand.inputDeviceHandPart.inputDeviceHandPartUI.modelHand;
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            OnUpdateHandTransform();
            OnUpdateHandNormal();
            OnUpdateHandRight();
            OnUpdateHandCenter();
        }
        protected virtual void OnUpdateHandTransform() {

            if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                inputDataGetHand.inputDeviceHandPart.inputDataHand.position = SvrManager.Instance.head.transform.TransformPoint(inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.localPosition + inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.positionOffest);
                inputDataGetHand.inputDeviceHandPart.inputDataHand.rotation =Quaternion.Euler( SvrManager.Instance.head.transform.eulerAngles + inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.eulerAnglesOffset);
                //DebugMy.Log("handInfo:" + handInfo.position + "::" + SvrManager.Instance.leftCamera.transform.position, this);
            }
        }

        protected virtual void OnUpdateHandRight() {
            Vector3 v1, v2;
            if(modelHand.ActiveHandModel != null) {
                v1 = modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).transform.position ;
                v2 = modelHand.ActiveHandModel.GetJointTransform(FINGER.ring, JOINT.Four).transform.position;
                inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.right = Vector3.Lerp(inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.right, (v2-v1).normalized, 0.8f);
            }

        }
        protected virtual void OnUpdateHandNormal() {
            Vector3 v1, v2;
            if(modelHand.ActiveHandModel != null) {
                v1 = modelHand.ActiveHandModel.GetJointTransform(FINGER.ring, JOINT.Four).position - modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).transform.position;
                v2 = modelHand.ActiveHandModel.GetJointTransform(FINGER.small, JOINT.Five).position - modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).position;

                if(inputDataGetHand.inputDeviceHandPart.PartType == InputDevicePartType.HandLeft) {
                    inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.normal = Vector3.Lerp(inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.normal, Vector3.Cross(v1, v2).normalized, 0.8f);
                } else if(inputDataGetHand.inputDeviceHandPart.PartType == InputDevicePartType.HandRight) {
                    inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.normal = Vector3.Lerp(inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.normal, Vector3.Cross(v2, v1).normalized, 0.8f);
                }
            }

        }
        protected virtual void OnUpdateHandCenter() {
            Vector3 v1, v2, v3;
            if(modelHand.ActiveHandModel != null) {
                v1 = modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).position;
                v2 = modelHand.ActiveHandModel.GetJointTransform(FINGER.small, JOINT.Four).position;
                v3 = modelHand.ActiveHandModel.GetJointTransform(FINGER.small, JOINT.Five).position;
                inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.centerPosition = Vector3.Lerp(inputDataGetHand.inputDeviceHandPart.inputDataHand.handInfo.centerPosition, (v1 + v2 + v3) / 3f,0.8f);
            }

        }

    }
}
