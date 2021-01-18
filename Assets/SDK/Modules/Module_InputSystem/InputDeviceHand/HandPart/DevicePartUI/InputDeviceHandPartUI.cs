using System.Collections;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class InputDeviceHandPartUI : InputDevicePartUIBase {
        public InputDeviceHandPart inputDeviceHandPart {
            get {
                return inputDevicePartBase as InputDeviceHandPart;
            }
        }

        public ModelHand modelHand {
            get {
                return modelBase as ModelHand;
            }
        }

        HandMenu _handMenu;
        public HandMenu handMenu {
            get {
                if(_handMenu == null) {
                    _handMenu = GetComponentInChildren<HandMenu>(true);
                }
                return _handMenu;
            }
        }

        public override void OnSCAwake() {
            base.OnSCAwake();
            AddModule(handMenu);
        }
        //public override void OnSCStart() {
        //    base.OnSCStart();
        //    handMenu.ModuleStart();
        //}

        Vector3 handHead;
        float angle = 0;
        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            if(inputDeviceHandPart && SvrManager.Instance) {
                handHead = SvrManager.Instance.head.position - inputDeviceHandPart.inputDataHand.handInfo.centerPosition;
                angle = Vector3.Angle(handHead, inputDeviceHandPart.inputDataHand.handInfo.normal);
                if(angle < 30) {
                    if(handMenu && handMenu.IsModuleStarted == false) {
                        handMenu.ModuleStart();
                    }
                } else if(angle > 60) {
                    if(handMenu && handMenu.IsModuleStarted) {
                        handMenu.ModuleStop();
                    }
                }
            }
        }
    }
}