using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class InputDataGetHandKey : InputDataGetKey {

        public InputDataGetHand inputDataGetHand;
        public InputDataGetHandKey(InputDataGetHand _inputDataGetHand) : base(_inputDataGetHand) {
            inputDataGetHand = _inputDataGetHand;
        }

        public override void OnSCAwake() {
            base.OnSCAwake();
            InputDeviceHandPartEventBase.eventDelegate += CatchEvent;
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            inputDataGetHand = null; 
            InputDeviceHandPartEventBase.eventDelegate -= CatchEvent;
        }

        public override void OnUpdateKey() {

            ///Touch事件转化为Key
            //if(inputDataGetHand.inputDeviceHandPart.gGT26DofDetector.gGT26DofPointer.targetDetectMode == HandPointer.TargetDetectMode.NearTouch) {
            //    GetKeyEventByDistance(inputDataGetHand.inputDeviceHandPart.inputDataHand.SCPointEventData);
            //}
        }

        void CatchEvent(InputDeviceHandPart inputDeviceHandPart, HandEventType eventType) {
            if(eventType == HandEventType.CatchDown && inputDeviceHandPart == inputDataGetHand.inputDeviceHandPart) {
                inputDataGetHand.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Enter, InputKeyState.DOWN);
            } else if(eventType == HandEventType.CatchUp && inputDeviceHandPart == inputDataGetHand.inputDeviceHandPart) {
                inputDataGetHand.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Enter, InputKeyState.UP);
            }
        }


    }
}
