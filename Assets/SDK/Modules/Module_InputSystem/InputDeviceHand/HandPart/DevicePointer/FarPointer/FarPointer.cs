using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class FarPointer : PointerBase {
        public HandDetector handDetector {
            get {
                return detectorBase as HandDetector;
            }
        }

        [HideInInspector]
        public float FarDetectDistance = 30f;
        public override PointerType PointerType { get => PointerType.Far; }


        public Action<bool> TargetDetectModelChange;


        protected override void UpdateTransform() {
            transform.position = handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).position;
            transform.rotation = SvrManager.Instance.leftCamera.transform.rotation;
        }

        protected override void DoTargetDetect() {
                SCInputModule.Instance.ProcessCS(handDetector.inputDevicePartBase.inputDataBase.SCPointEventData, transform, LayerMask, FarDetectDistance);
                IsFocusLocked = handDetector.inputDevicePartBase.inputDataBase.SCPointEventData.DownPressGameObject != null;
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            IsFocusLocked = false;
        }
    }
}
