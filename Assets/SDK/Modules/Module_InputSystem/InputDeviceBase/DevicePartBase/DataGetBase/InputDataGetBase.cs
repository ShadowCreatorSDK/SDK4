using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDataGetBase : SCModule {

        /// <summary>
        /// 输入设备part
        /// </summary>
        public InputDevicePartBase inputDevicePartBase;
        public InputDataGetBase(InputDevicePartBase _inputDevicePartBase) {
            inputDevicePartBase = _inputDevicePartBase;
        }

        public List<InputDataGetOneBase> dataGetOneList = new List<InputDataGetOneBase>();


        #region Module Behavior
        public override void OnSCAwake() {
            base.OnSCAwake();
            foreach(var item in dataGetOneList) {
                AddModule(item);
            }
        }

        public override void OnSCStart() {
            base.OnSCStart();
            foreach(var item in dataGetOneList) {
                item.ModuleStart();
            }
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();

            inputDevicePartBase = null;
            dataGetOneList = null;
        }
        #endregion
    }
}
