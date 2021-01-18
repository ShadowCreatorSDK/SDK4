using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDevicePartDispatchEventBase : SCModule {

        public InputDevicePartBase inputDevicePartBase;
        public InputDevicePartKeyEvent inputDevicePartKeyEvent;
        public InputDevicePartDispatchEventBase(InputDevicePartBase inputDevicePartBase) {
            this.inputDevicePartBase = inputDevicePartBase;
            eventList.Add(inputDevicePartKeyEvent = new InputDevicePartKeyEvent(this));
        }

        public DispatcherBase dispatcherBase;
        public List<InputDevicePartEventBase> eventList = new List<InputDevicePartEventBase>();

        public override void OnSCAwake() {
            base.OnSCAwake();

            AddModule(dispatcherBase);

            foreach(var item in eventList) {
                AddModule(item);
            }
        }

        public override void OnSCStart() {
            base.OnSCStart();

            dispatcherBase?.ModuleStart();

            foreach(var item in eventList) {
                item.ModuleStart();
            }
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();

            dispatcherBase = null;
            eventList = null;
        }

    }
}
