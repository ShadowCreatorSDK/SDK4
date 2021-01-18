using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class InputDevicePartBase : SCModuleMono {

        /// <summary>
        /// 自行赋值
        /// </summary>
        [Header("Select Right Type")]
        public InputDevicePartType PartType;

        /// <summary>
        /// 输入设备Part所属Device模块
        /// </summary>;
        InputDeviceBase _inputDeviceBase;
        public InputDeviceBase inputDeviceBase {
            get {
                if(_inputDeviceBase == null) {
                    _inputDeviceBase = GetComponentInParent<InputDeviceBase>();
                }
                return _inputDeviceBase;
            }
            private set {
                _inputDeviceBase = value;
            }
        }


        /// <summary>
        /// 输入设备PartUI模块
        /// </summary>;
        InputDevicePartUIBase _inputDevicePartUIBase;
        public InputDevicePartUIBase inputDevicePartUIBase {
            get {
                if(_inputDevicePartUIBase == null) {
                    _inputDevicePartUIBase = GetComponentInChildren<InputDevicePartUIBase>(true);
                }
                return _inputDevicePartUIBase;
            }
            private set {
                _inputDevicePartUIBase = value;
            }
        }


        /// <summary>
        /// 目标检测及Cursor显示模块
        /// </summary>
        DetectorBase mDetectorBase;
        public DetectorBase detectorBase {
            get {
                if(mDetectorBase == null) {
                    mDetectorBase = GetComponentInChildren<DetectorBase>(true);
                }
                return mDetectorBase;
            }
            private set {
                mDetectorBase = value;
            }
        }

        /// <summary>
        /// 数据获取模块
        /// </summary>
        public InputDataBase inputDataBase { get; set; }

        /// <summary>
        /// 数据获取模块
        /// </summary>
        public InputDataGetBase inputDataGetBase { get; set; }

        /// <summary>
        /// 设备状态judge模块
        /// </summary>
        public InputDevicePartStatusBase inputDevicePartStatusBase { get; set; }

        /// <summary>
        /// 设备Dispatch Event模块
        /// </summary>
        public InputDevicePartDispatchEventBase inputDevicePartDispatchEventBase { get; set; }
        


        public override void OnSCAwake() {
            base.OnSCAwake();

            ModuleCreater();

            AddModule(inputDataBase);
            AddModule(inputDataGetBase);
            AddModule(inputDevicePartStatusBase);
            AddModule(inputDevicePartDispatchEventBase);
            AddModule(inputDevicePartUIBase);
            AddModule(detectorBase);

        }

        protected abstract void ModuleCreater();

        public override void OnSCStart() {
            base.OnSCStart();

            if(inputDataBase != null)
                inputDataBase.ModuleStart();

            if(inputDataGetBase != null)
                inputDataGetBase.ModuleStart();

            if(inputDevicePartStatusBase != null)
                inputDevicePartStatusBase.ModuleStart();

            //if(inputDevicePartDispatchEventBase != null)
            //    inputDevicePartDispatchEventBase.ModuleStart();

            //if(inputDevicePartUIBase != null)
            //    inputDevicePartUIBase.ModuleStart();

            //if(pointerBase != null)
            //    pointerBase.ModuleStart();

        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(inputDataBase.isVaild == false) {

                if(inputDevicePartUIBase != null && inputDevicePartUIBase.IsModuleStarted)
                    inputDevicePartUIBase.ModuleStop();

                if(detectorBase != null && detectorBase.IsModuleStarted)
                    detectorBase.ModuleStop();

                if(inputDevicePartDispatchEventBase != null && inputDevicePartDispatchEventBase.IsModuleStarted)
                    inputDevicePartDispatchEventBase.ModuleStop();
            } else {


                if(inputDevicePartUIBase != null && !inputDevicePartUIBase.IsModuleStarted)
                    inputDevicePartUIBase.ModuleStart();

                if(detectorBase != null && !detectorBase.IsModuleStarted)
                    detectorBase.ModuleStart();

                if(inputDevicePartDispatchEventBase != null && !inputDevicePartDispatchEventBase.IsModuleStarted)
                    inputDevicePartDispatchEventBase.ModuleStart();
            }

        }


        public override void OnSCDestroy() {
            base.OnSCDestroy();

            inputDataBase = null;

            inputDataGetBase = null;

            inputDevicePartStatusBase = null;

            inputDevicePartDispatchEventBase = null;

            inputDevicePartUIBase = null;

            detectorBase = null;
        }
    }
}
