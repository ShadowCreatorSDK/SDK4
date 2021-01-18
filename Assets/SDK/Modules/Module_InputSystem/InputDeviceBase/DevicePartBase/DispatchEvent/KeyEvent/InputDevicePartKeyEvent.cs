using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem {

    public class InputDevicePartKeyEvent : InputDevicePartEventBase {

        public InputDevicePartKeyEvent(InputDevicePartDispatchEventBase inputDevicePartDispatchEventBase) :base(inputDevicePartDispatchEventBase) {
        }

        protected override void DispatchEventDelegate() {
            foreach(var item in inputDataBase.inputKeys.inputKeyPressDic) {

                if(item.Value == InputKeyState.DOWN) {
                    DispatcherBase.KeyDownDelegateInvoke(item.Key, inputDevicePartDispatchEventBase.inputDevicePartBase);
                } else if(item.Value == InputKeyState.UP) {
                    DispatcherBase.KeyUpDelegateInvoke(item.Key, inputDevicePartDispatchEventBase.inputDevicePartBase);
                } else if(item.Value == InputKeyState.LONG) {
                    DispatcherBase.KeyLongDelegateInvoke(item.Key, inputDevicePartDispatchEventBase.inputDevicePartBase);
                }
            }
        }

        protected override void DispatchEventTarget() {
           
        }

        protected override void OnUpdateEvent() {
           
        }
    }
}
