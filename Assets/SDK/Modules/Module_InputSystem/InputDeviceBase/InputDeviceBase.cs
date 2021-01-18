using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class InputDeviceBase : SCModuleMono {

        /// <summary>
        /// 拖拽赋值
        /// </summary>
        public List<InputDevicePartBase> _inputDevicePartList;
        public List<InputDevicePartBase> inputDevicePartList {
            get {
                if(_inputDevicePartList == null || _inputDevicePartList.Count == 0) {
                    _inputDevicePartList = GetComponentsInChildren<InputDevicePartBase>(true).ToList();
                }
                return _inputDevicePartList;
            }
            private set {
                _inputDevicePartList = value;
            }
        }
        /// <summary>
        /// 拖拽赋值
        /// </summary>
        [SerializeField]
        private InputDeviceUIBase _inputDeviceUI;
        public InputDeviceUIBase inputDeviceUI {
            get {
                if (_inputDeviceUI == null) {
                    _inputDeviceUI = GetComponentInChildren<InputDeviceUIBase>(true);
                }
                return _inputDeviceUI;
            }
            private set {
                _inputDeviceUI = value;
            }
        }





        /// <summary>
        /// 输入设备类型
        /// </summary>
        public abstract InputDeviceType inputDeviceType { get; }


        public override void OnSCAwake() {
            base.OnSCAwake();
            foreach(var item in inputDevicePartList) {
                AddModule(item);
            }
            AddModule(inputDeviceUI);
        }


        public override void OnSCStart() {
            base.OnSCStart();
            InputDeviceStart();
            inputDeviceUI?.ModuleStart();
        }

        protected virtual void InputDeviceStart() {
            foreach(var item in inputDevicePartList) {
                item.ModuleStart();
            }
        }


        public override void OnSCDisable() {
            base.OnSCDisable();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            inputDevicePartList = null;
            inputDeviceUI = null;
        }

        public void SetActiveInputDevicePart(InputDevicePartType partType, bool active) {
            if(active) {
                foreach(var devicePart in inputDevicePartList) {
                    if(devicePart.PartType == partType) {
                        if(false == devicePart.IsModuleStarted) {
                            devicePart.ModuleStart();
                        }
                        break;
                    }
                }
            } else {
                foreach(var devicePart in inputDevicePartList) {
                    if(devicePart.PartType == partType) {
                        if(devicePart.IsModuleStarted) {
                            devicePart.ModuleStop();
                        }
                        break;
                    }
                }
            }
        }



        public virtual bool IsAnyPartVaild {
            get {
                foreach(var part in inputDevicePartList) {
                    if(part.inputDataBase.isVaild) {
                        return true;
                    }
                }
                return false;
            }
        }

    }
}
