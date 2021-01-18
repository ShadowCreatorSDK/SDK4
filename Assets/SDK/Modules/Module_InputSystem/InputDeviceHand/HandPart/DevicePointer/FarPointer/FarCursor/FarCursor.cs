using UnityEngine;
using DG.Tweening;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class FarCursor : DefaultCursor {

        public FarPointer farPointer {
            get {
                return Transition<FarPointer>(pointerBase);
            }
        }

        public ProximityLight ProximityLight;

        public override void OnSCStart() {
            base.OnSCStart();
            farPointer.TargetDetectModelChange += TargetDetectModelChange;

            farPointer.handDetector.inputDeviceHandPart.inputDataHand.SCPointEventData.TouchPressPercentDelegate += TouchPressPercent;
        }

        public override void UpdateTransform() {
            if(farPointer.PointerType == PointerType.Touch) {
                //transform.position = farPointer.handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.TouchCursor.position;
                //transform.rotation = farPointer.handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.TouchCursor.rotation;

                //if(lineIndicateBase.IsModuleStarted) {
                //    lineIndicateBase.ModuleStop();
                //}

            } else {
                base.UpdateTransform();
                //if(!lineIndicateBase.IsModuleStarted) {
                //    lineIndicateBase.ModuleStart();
                //}
            }
        }

        Vector3 initLocalScale;

        public override void InitCursorPart() {
            if(farPointer.PointerType == PointerType.Touch) {
                CursorPartModuleStart(CursorPartType.FingerFocus);
            } else {
                CursorPartModuleStart(CursorPartType.Focus);
            }
        }

        void TouchPressPercent(float percent,float hitObjWeight) {
            transform.localScale = Mathf.Clamp(1 - percent, pressLocalScale, 1) * initLocalScale;
        }

        void TargetDetectModelChange(bool isNear) {
            if(isNear) {
                CusrorPartNormalStart(CursorPartType.FingerFocus);
            } else {
                CusrorPartNormalStart(CursorPartType.Focus);
            }
        }

        public override void UpdateNormalCursorVisual() {

            if(farPointer.detectorBase.inputDevicePartBase.inputDataBase.inputKeys.GetKeyDown(InputKeyCode.Enter)) {
                
                ProximityLight?.Pulse();

                if(farPointer.PointerType == PointerType.Touch) {
                    CusrorPartNormalStart(CursorPartType.FingerPress);
                } else {
                    CusrorPartNormalStart(CursorPartType.Press);
                }

                transform.localScale = initLocalScale * pressLocalScale;

            } else if(farPointer.detectorBase.inputDevicePartBase.inputDataBase.inputKeys.GetKeyUp(InputKeyCode.Enter)) {
                if(farPointer.PointerType == PointerType.Touch) {
                    CusrorPartNormalStart(CursorPartType.FingerFocus);
                } else {
                    CusrorPartNormalStart(CursorPartType.Focus);
                }

                transform.localScale = initLocalScale;
            }

        }

        public override void OnSCAwake() {
            base.OnSCAwake();

            initLocalScale = transform.localScale;
        }
        public override void OnSCDestroy() {
            base.OnSCDestroy();
            //HandTargetDetect.HandPointer.inputDeviceHandPart.inputDataHand.SCPointEventData.TouchPressPercentDelegate -= TouchPressPercent;
        }
    }
}
