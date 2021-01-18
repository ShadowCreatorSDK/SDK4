using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDevicePartEventBase : SCModule {

        public InputDevicePartDispatchEventBase inputDevicePartDispatchEventBase;

        public InputDevicePartEventBase(InputDevicePartDispatchEventBase inputDevicePartDispatchEventBase) {
            this.inputDevicePartDispatchEventBase = inputDevicePartDispatchEventBase;
        }


        public PointerBase pointerBase {
            get {
                return inputDevicePartDispatchEventBase.inputDevicePartBase.detectorBase.pointerBase;
            }
        }

        public InputDataBase inputDataBase {
            get {
                return inputDevicePartDispatchEventBase.inputDevicePartBase.inputDataBase;
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            OnUpdateEvent();
            DispatchEventDelegate();
            DispatchEventTarget();
        }

        protected abstract void OnUpdateEvent();
        protected abstract void DispatchEventDelegate();
        protected abstract void DispatchEventTarget();
    }
}
