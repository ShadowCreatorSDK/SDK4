
using SC.XR.Unity.Module_SDKVersion;
using SC.XR.Unity.Module_Device;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC.XR.Unity.Module_SkyBox;

public class API_Module_SkyBox {
    public static Material GetSkyBox(SkyBoxType type) {
        return Module_SkyBox.getInstance.GetSkyBox(type);
    }

}