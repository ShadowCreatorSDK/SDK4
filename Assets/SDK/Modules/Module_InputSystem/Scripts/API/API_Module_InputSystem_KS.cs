using System;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS;
using UnityEngine.EventSystems;

public class API_Module_InputSystem_KS {

    ///API-No.150
    /// <summary>
    /// 获取InputSystem支持的手柄输入设备,手柄输入设备包含二个Part，名曰：BTRight,BTLeft,也就是第一个手柄和第二个手柄
    /// </summary>
    /// <returns>null表示不支持或者InputSystem未初始好</returns>
    public static InputDeviceKS KSDevice {
        get {
            if(Module_InputSystem.instance) {
                return Module_InputSystem.instance.GetInputDevice<InputDeviceKS>(InputDeviceType.KS);
            }
            return null;
        }
    }

    ///API-No.151
    /// <summary>
    /// 手柄输入设备连接的第一个手柄
    /// </summary>
    public static InputDeviceKSPart KSRight {
        get {
            if (KSDevice && KSDevice.inputDevicePartList.Count > 0) {
                foreach (var part in KSDevice.inputDevicePartList) {
                    if (part.PartType == InputDevicePartType.KSRight) {
                        return part as InputDeviceKSPart;
                    }
                }
            }
            
            return null;
        }
    }

    ///API-No.152
    /// <summary>
    /// 手柄输入设备连接的第二个手柄
    /// </summary>
    public static InputDeviceKSPart KSLeft {
        get {
            if (KSDevice && KSDevice.inputDevicePartList.Count > 0) {
                foreach (var part in KSDevice.inputDevicePartList) {
                    if (part.PartType == InputDevicePartType.KSLeft) {
                        return part as InputDeviceKSPart;
                    }
                }
            }
            return null;
        }
    }

    public enum GCType { 
        Left,
        Right
    }

    ///API-No.153
    /// <summary>
    /// BTRight/BTLeft的四元数，全局坐标
    /// </summary>
    /// <param name="type">右手柄 BTRight /  左手柄 BTLeft</param>
    /// <returns></returns>
    public static Quaternion KSRotation(GCType type = GCType.Right) {
        if(KSRight && type == GCType.Right) {
            return KSRight.inputDataKS.rotation;
        } else if(KSLeft && type == GCType.Left) {
            return KSLeft.inputDataKS.rotation;
        }
        return Quaternion.identity;
    }

    ///API-No.154
    /// <summary>
    /// BTRight/BTLeft的位置信息，全局坐标
    /// </summary>
    /// <param name="type">右手柄 BTRight /  左手柄 BTLeft</param>
    /// <returns></returns>
    public static Vector3 KSPosition(GCType type = GCType.Right) {
        if(KSRight && type == GCType.Right) {
            return KSRight.inputDataKS.position;
        } else if(KSLeft && type == GCType.Left) {
            return KSLeft.inputDataKS.position;
        }
        return Vector3.zero;
    }

    public static Transform KSTransform(GCType type = GCType.Right) {
        if (KSRight && type == GCType.Right) {
            return KSRight.inputDeviceKSPartUI.ModelGC.transform;
        } else if (KSLeft && type == GCType.Left) {
            return KSLeft.inputDeviceKSPartUI.ModelGC.transform;
        }
        return null;
    }


}
