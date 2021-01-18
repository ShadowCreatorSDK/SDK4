using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class ModelBase : SCModuleMono {

        InputDevicePartUIBase _inputDevicePartUIBase;
        public InputDevicePartUIBase inputDevicePartUIBase {
            get {
                if(_inputDevicePartUIBase == null) {
                    _inputDevicePartUIBase = GetComponentInParent<InputDevicePartUIBase>();
                }
                return _inputDevicePartUIBase;
            }
        }


    }
}