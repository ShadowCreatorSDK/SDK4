using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class DetectorBase : SCModuleMono {

        public PointerBase currentPointer;

        InputDevicePartBase _inputDevicePartBase;
        public InputDevicePartBase inputDevicePartBase {
            get {
                if(_inputDevicePartBase == null) {
                    _inputDevicePartBase = GetComponentInParent<InputDevicePartBase>();
                }
                return _inputDevicePartBase;
            }
            set {
                _inputDevicePartBase = value;
            }
        }

        [SerializeField]
        PointerBase mPointerBase;
        public PointerBase pointerBase {
            get {
                if(mPointerBase == null) {
                    mPointerBase = GetComponentInChildren<PointerBase>(true);
                }
                return mPointerBase;
            }
            set {
                mPointerBase = value;
            }
        }



        public override void OnSCAwake() {
            base.OnSCAwake();

            AddModule(pointerBase);
        }


        public override void OnSCStart() {
            base.OnSCStart();

            pointerBase?.ModuleStart();
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            currentPointer = pointerBase;
        }


        public override void OnSCDestroy() {
            base.OnSCDestroy();

            pointerBase = null;
        }
    }
}
