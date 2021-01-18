using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class InputDeviceUIBase : SCModuleMono {

        private InputDeviceBase _inputDevice;
        public InputDeviceBase inputDevice {
            get {
                if(_inputDevice == null) {
                    _inputDevice = GetComponentInParent<InputDeviceBase>();
                }
                return _inputDevice;
            }
        }

        [Header("Drag For Initialize")]
        [SerializeField]
        protected List<SCModuleMono> UIModuleList;

        public override void OnSCAwake() {
            base.OnSCAwake();
            foreach (var item in UIModuleList) {
                AddModule(item);
            }
        }

    }
}
