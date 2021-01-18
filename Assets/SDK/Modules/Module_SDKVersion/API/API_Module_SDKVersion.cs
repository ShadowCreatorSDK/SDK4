
using SC.XR.Unity.Module_SDKVersion;
using SC.XR.Unity.Module_Device;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_Module_SDKVersion {
    public static string Version {
        get {
            return Module_SDKVersion.getInstance.GetVersion;
        }
    }

}