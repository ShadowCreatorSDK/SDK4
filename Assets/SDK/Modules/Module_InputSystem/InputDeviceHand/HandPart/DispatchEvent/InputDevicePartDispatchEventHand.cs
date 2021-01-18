using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDevicePartDispatchEventHand : InputDevicePartDispatchEventBase {

        public InputDeviceHandPart inputDeviceHandPart;

        public InputDeviceHandPartCatchEvent inputDeviceHandPartCatchEvent;
        public InputDeviceHandPartLeftRightEvent inputDeviceHandPartLeftRightEvent;
        public InputDeviceHandPartTurnLeftRightEvent inputDeviceHandPartTurnLeftRightEvent;

        public InputDevicePartDispatchEventHand(InputDeviceHandPart inputDeviceHandPart) : base(inputDeviceHandPart) {
            this.inputDeviceHandPart = inputDeviceHandPart;
            eventList.Add(inputDeviceHandPartCatchEvent = new InputDeviceHandPartCatchEvent(this));
            //eventList.Add(inputDeviceHandPartLeftRightEvent = new InputDeviceHandPartLeftRightEvent(this));
            //eventList.Add(inputDeviceHandPartTurnLeftRightEvent = new InputDeviceHandPartTurnLeftRightEvent(this));
        }
    }
}
