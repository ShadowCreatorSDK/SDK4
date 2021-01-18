
using SC.XR.Unity.Module_Device;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_Module_Device {
    public static DeviceBase Current {
        get {
            return Module_Device.getInstance.Current;
        }
    }

    public static AndroidDevice CurrentAndroid {
        get {
            return Module_Device.getInstance.CurrentAndroid;
        }
    }

    public static bool IsOurGlass {
        get {
            if (CurrentAndroid != null) {
                return CurrentAndroid.type == AndroidDeviceType.Other ? false : true;
            }
            return false;
        }
    }

    public static bool IsPhoneDevice {
        get {
            if (!Application.isEditor) {
                if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
                    return true;
                }
            }
            return false;
        }
    }

}