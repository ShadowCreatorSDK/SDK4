using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class HandDetector : DetectorBase {

        private IPointer handPointer;

        public InputDeviceHandPart inputDeviceHandPart {
            get {
                return inputDevicePartBase as InputDeviceHandPart;
            }
        }

        public FarPointer farPointer {
            get {
                return Transition<FarPointer>(pointerBase);
            }
        }

        [SerializeField]
        TouchPointer mTouchPointer;
        public TouchPointer touchPointer {
            get {
                if(mTouchPointer == null) {
                    mTouchPointer = GetComponentInChildren<TouchPointer>(true);
                }
                return mTouchPointer;
            }
            set {
                mTouchPointer = value;
            }
        }

        [SerializeField]
        GrabPointer mGrabPointer;
        public GrabPointer grabPointer {
            get {
                if(mGrabPointer == null) {
                    mGrabPointer = GetComponentInChildren<GrabPointer>(true);
                }
                return mGrabPointer;
            }
            set {
                mGrabPointer = value;
            }
        }



        public override void OnSCAwake() {
            base.OnSCAwake();
            AddModule(touchPointer);
            AddModule(grabPointer);
        }

        public override void OnSCStart() {
            //base.OnSCStart();
            //touchPointer?.ModuleStart();
            //grabPointer?.ModuleStart();
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(touchPointer.IsFocusLocked || grabPointer.IsFocusLocked || farPointer.IsFocusLocked) {

            } else {
                if(touchPointer.FindClosestTouchableForLayerMask()) {
                    if(touchPointer.IsModuleStarted == false) {
                        touchPointer.ModuleStart();
                        handPointer = touchPointer;
                    }
                    if(grabPointer.IsModuleStarted) {
                        grabPointer.ModuleStop();
                    }
                    if(farPointer.IsModuleStarted) {
                        farPointer.ModuleStop();
                    }
                } else if(grabPointer.FindClosestGrabbableForLayerMask()) {
                    if(touchPointer.IsModuleStarted ) {
                        touchPointer.ModuleStop();
                    }
                    if(grabPointer.IsModuleStarted == false) {
                        grabPointer.ModuleStart();
                        handPointer = grabPointer;
                    }
                    if(farPointer.IsModuleStarted) {
                        farPointer.ModuleStop();
                    }
                } else {
                    if(touchPointer.IsModuleStarted) {
                        touchPointer.ModuleStop();
                    }
                    if(grabPointer.IsModuleStarted ) {
                        grabPointer.ModuleStop();
                    }
                    if(farPointer.IsModuleStarted == false) {
                        farPointer.ModuleStart();
                        handPointer = farPointer;
                    }
                }
            }
            currentPointer = handPointer;

        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            touchPointer = null;
            grabPointer = null;
        }
    }
}
