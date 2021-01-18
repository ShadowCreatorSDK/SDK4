
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDevicePartStatusBase : SCModule {

        
        public InputDevicePartBase inputDevicePartBase;
        public InputDevicePartStatusBase(InputDevicePartBase _inputDevicePartBase) {
            inputDevicePartBase = _inputDevicePartBase;
        }
    }
}
