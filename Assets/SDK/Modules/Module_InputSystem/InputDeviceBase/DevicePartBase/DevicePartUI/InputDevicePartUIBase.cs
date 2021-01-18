using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDevicePartUIBase : SCModuleMono {

        /// <summary>
        /// 输入设备Part所属Device模块
        /// </summary>;
        InputDevicePartBase _inputDevicePartBase;
        public InputDevicePartBase inputDevicePartBase {
            get {
                if(_inputDevicePartBase == null) {
                    _inputDevicePartBase = GetComponentInParent<InputDevicePartBase>();
                }
                return _inputDevicePartBase;
            }
        }

        ModelBase _modelBase;
        public ModelBase modelBase {
            get {
                if(_modelBase == null) {
                    _modelBase = GetComponentInChildren<ModelBase>(true);
                }
                return _modelBase;
            }
            protected set {
                _modelBase = value;
            }
        }

        #region Module Behavior

        public override void OnSCAwake() {
            base.OnSCAwake();
            AddModule(modelBase);
        }


        public override void OnSCStart() {
            base.OnSCStart();
            modelBase?.ModuleStart();
        }

        public override void OnSCLateUpdate() {
            
            UpdateTransform();

            base.OnSCLateUpdate();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            _modelBase = null;
            _inputDevicePartBase = null;
        }

        #endregion

        protected virtual void UpdateTransform() {
            transform.position = inputDevicePartBase.inputDataBase.position;
            transform.rotation = inputDevicePartBase.inputDataBase.rotation;
        }

    }
}
