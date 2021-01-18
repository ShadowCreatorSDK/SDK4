
using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_Device {

    [Serializable]
    public class IOSDevice : DeviceBase {

        /// <summary>
        /// 设备型号
        /// </summary>
        /// <returns></returns>
        public override string MODEL {
            get {
                return "IOS";
            }
        }

        /// <summary>
        /// SN号
        /// </summary>
        public override string SN {
            get {
                return "000";
            }
        }

        /// <summary>
        /// Release_Vesion
        /// </summary>
        public override string RELEASE_VERSION {
            get {
                return "000";
            }
        }

        /// <summary>
        /// BatteryLevel
        /// </summary>
        public override int BatteryLevel {
            get {
                return 60;
            }
        }

    }

}