using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public class InputDeviceGCPartEventTouchTP : InputDeviceGCPartEventBase {

        InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC;
        public InputDeviceGCPartEventTouchTP(InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC) : base(inputDevicePartDispatchEventGC) {
            this.inputDevicePartDispatchEventGC = inputDevicePartDispatchEventGC;
        }


        protected override void OnUpdateEvent() {

            if(inputDevicePartDispatchEventGC.inputDeviceGCPart.inputDataGC.isTpTouch == true) {

                if(previousEvent == GCEventType.TouchDown || previousEvent == GCEventType.TouchDrag) {
                    currentEvent = GCEventType.TouchDrag;
                } else {
                    currentEvent = GCEventType.TouchDown;
                }

            } else {
                if(previousEvent == GCEventType.TouchDown || previousEvent == GCEventType.TouchDrag) {
                    currentEvent = GCEventType.TouchUp;
                } else {
                    currentEvent = GCEventType.Null;
                }
            }

            previousEvent = currentEvent;

        }
    }
}
