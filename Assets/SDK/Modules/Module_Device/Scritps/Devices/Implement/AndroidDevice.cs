
using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_Device {

    [Serializable]
    public class AndroidDevice : DeviceBase {


        public AndroidDeviceType type;


        /// <summary>
        /// 设备型号
        /// </summary>
        /// <returns></returns>
        public override string MODEL {
            get {
                AndroidJavaClass os = new AndroidJavaClass("android.os.Build");
                return os.GetStatic<string>("MODEL");
            }
        }

        /// <summary>
        /// SN号
        /// </summary>
        public override string SN {
            get {
                AndroidJavaClass os = new AndroidJavaClass("android.os.Build");
                return os.GetStatic<string>("SERIAL");
            }
        }

        /// <summary>
        /// Release_Vesion
        /// </summary>
        public override string RELEASE_VERSION {
            get {
                AndroidJavaClass os = new AndroidJavaClass("android.os.Build$VERSION");
                return os.GetStatic<string>("RELEASE");
            }
        }

        /// <summary>
        /// BatteryLevel
        /// </summary>
        public override int BatteryLevel {
            get {
                try {
                    string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
                    return int.Parse(CapacityString);
                } catch (Exception e) {
                    Debug.Log("Failed to read battery power; " + e.Message);
                }
                return 60;
            }
        }

        public override void ShowInfo() {
            base.ShowInfo();
            DebugMy.Log(" *** Device Info *** "
                                + "  DeviceType:" + type
                                , this, true);
        }
    }


}