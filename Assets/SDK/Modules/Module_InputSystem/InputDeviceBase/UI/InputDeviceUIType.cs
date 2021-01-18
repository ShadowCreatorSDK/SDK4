using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class InputDeviceUIType : SCModuleMono {

        private InputDeviceUIBase _inputDeviceUI;
        public InputDeviceUIBase inputDeviceUI {
            get {
                if(_inputDeviceUI == null) {
                    _inputDeviceUI = GetComponentInParent<InputDeviceUIBase>();
                }
                return _inputDeviceUI;
            }
        }

    }
}
