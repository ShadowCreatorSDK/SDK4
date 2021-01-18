using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("Please Use Module_Device")]
public class DeviceInfo {

    /// <summary>
    /// 设备型号
    /// </summary>
    /// <returns></returns>
    public static string MODEL {
        get{
            if(Application.platform == RuntimePlatform.Android) {
                AndroidJavaClass os = new AndroidJavaClass("android.os.Build");
                return os.GetStatic<string>("MODEL");
            }
            return "Standalone";
        }
    }

    /// <summary>
    /// SN号
    /// </summary>
    public static string SN {
        get {
            if(Application.platform == RuntimePlatform.Android) {
                AndroidJavaClass os = new AndroidJavaClass("android.os.Build");
                return os.GetStatic<string>("SERIAL");
            }
            return "Null";
        }
    }

    /// <summary>
    /// Release_Vesion
    /// </summary>
    public static string RELEASE_VERSION {
        get {
            if(Application.platform == RuntimePlatform.Android) {
                AndroidJavaClass os = new AndroidJavaClass("android.os.Build$VERSION");
                return os.GetStatic<string>("RELEASE");
            }
            return "Null";
        }
    }

    /// <summary>
    /// BatteryLevel
    /// </summary>
    public static int BatteryLevel {
        get {
            if (Application.platform == RuntimePlatform.Android) {
                try {
                    string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
                    return int.Parse(CapacityString);
                } catch (Exception e) {
                    Debug.Log("Failed to read battery power; " + e.Message);
                }
            }
            return 60;
        }
    }
}
