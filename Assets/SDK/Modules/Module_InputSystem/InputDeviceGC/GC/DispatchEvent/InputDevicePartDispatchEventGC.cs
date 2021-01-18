using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public abstract class InputDevicePartDispatchEventGC : InputDevicePartDispatchEventBase {

        public InputDeviceGCPart inputDeviceGCPart;

        public InputDeviceGCPartEventSliderLeftRight inputDevicePartEventSliderLeftRight;
        public InputDeviceGCPartEventSliderUpDown inputDevicePartEventSliderUpDown;
        public InputDeviceGCPartEventTouchTP inputDevicePartEventTouchTP;
        public InputDevicePartDispatchEventGC(InputDeviceGCPart inputDeviceGCPart) : base(inputDeviceGCPart) {
            this.inputDeviceGCPart = inputDeviceGCPart;
            eventList.Add(inputDevicePartEventSliderLeftRight = new InputDeviceGCPartEventSliderLeftRight(this));
            eventList.Add(inputDevicePartEventSliderUpDown = new InputDeviceGCPartEventSliderUpDown(this));
            eventList.Add(inputDevicePartEventTouchTP = new InputDeviceGCPartEventTouchTP(this));
        }
    }
}
