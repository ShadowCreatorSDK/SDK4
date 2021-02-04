using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDataBase : SCModule {

        public InputDevicePartBase inputDevicePartBase;
        public InputDataBase(InputDevicePartBase inputDevicePartBase) {
            this.inputDevicePartBase = inputDevicePartBase;
        }
        
        /// <summary>
        /// Key数据
        /// </summary>
        public InputKeys inputKeys;

        public InputKeyCode EnterKeyAlias = InputKeyCode.Trigger;
        public InputKeyCode CancelKeyAlias = InputKeyCode.Cancel;
        public InputKeyCode CalibrationKeyAlias = InputKeyCode.OTHER;

        bool _isVaild = false;

        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool isVaild {
            get {
                return _isVaild;
            }
            set {
                if(value != _isVaild) {
                    _isVaild = value;
                    DeviceVaildChanged(value);
                }
            }
        }

        /// <summary>
        /// 事件信息
        /// </summary>
        public SCPointEventData SCPointEventData;

        /// <summary>
        /// Posture,the inputDevice 6Dof Posture in the world
        /// </summary>
        public Vector3 position;
        public Quaternion rotation;

        #region Module Behavior
        public override void OnSCAwake() {
            base.OnSCAwake();
            isVaild = false;
            position = Vector3.zero;
            rotation = Quaternion.identity;
            SCPointEventData = new SCPointEventData(inputDevicePartBase, EventSystem.current);

            AddModule(inputKeys = new InputKeys(this));
        }

        public override void OnSCStart() {
            base.OnSCStart();
            inputKeys.ModuleStart();
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            isVaild = false;
            position = Vector3.zero;
            rotation = Quaternion.identity;
            SCPointEventData.Clear();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();

            inputKeys = null;
            SCPointEventData = null;
            inputDevicePartBase = null;
        }

        #endregion


        void DeviceVaildChanged(bool newVaild) {
            inputKeys.VaildChanged();
        }
    }
}
