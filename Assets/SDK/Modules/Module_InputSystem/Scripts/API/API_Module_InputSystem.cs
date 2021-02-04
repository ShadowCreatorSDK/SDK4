using System;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem;
public class API_Module_InputSystem
{

    ///API-No.50
    /// <summary>
    /// 获取Module_InputSystem的单例
    /// </summary>
    /// <returns></returns>
    public static Module_InputSystem GetInstance() {
        return Module_InputSystem.instance;
    }

    ///API-No.51
    /// <summary>
    /// Module_InputSystem是否初始化完成
    /// </summary>
    /// <returns>true 表示初始化完成，反之</returns>
    public static bool IsISInitialized() {
        if(Module_InputSystem.instance) {
            return Module_InputSystem.instance.initialize;
        }
        return false;
    }

    ///API-No.52
    /// <summary>
    /// 设置Module_InputSystem初始化完成时的回调
    /// </summary>
    /// <param name="action">委托的方法</param>
    public static void AddInitializedCallBack(Action action) {
        Module_InputSystem.instance.initializeCallBack += action;
    }

    ///API-No.53
    public static void RemoveInitializedCallBack(Action action) {
        Module_InputSystem.instance.initializeCallBack -= action;
    }

    ///API-No.54
    /// <summary>
    /// 使能某个输入设备，支持的输入设备见InputDeviceType
    /// </summary>
    /// <param name="inputDevice">输入设备</param>
    public static void EnableInputDevice(InputDeviceType inputDevice) {
        if(Module_InputSystem.instance) {
            Module_InputSystem.instance.SetActiveInputDevice(inputDevice,true);
        }
    }

    ///API-No.55
    /// <summary>
    /// 关闭某个输入设备，支持的输入设备见InputDeviceType
    /// </summary>
    /// <param name="inputDevice">输入设备</param>
    public static void DisableInputDevice(InputDeviceType inputDevice) {
        if(Module_InputSystem.instance) {
            Module_InputSystem.instance.SetActiveInputDevice(inputDevice,false);
        }
    }

    ///API-No.56
    ///监听某个按键事件方式1
    ///usingusing SC.XR.Unity.Module_InputSystem;  然后继承PointerHandlers类并重写需要的方法
    ///支持的事件有：
    ///OnPointerExit, OnPointerEnter, OnPointerDown,OnPointerClick,OnPointerUp, OnDrag


    ///API-No.57
    ///监听某个按键事件方式2
    ///using SC.XR.Unity.Module_InputSystem;  然后继承PointerDelegate类然后重写对应的事件
    ///支持的事件有：
    ///KeyUpDelegateUnRegister KeyUpDelegateRegister KeyLongDelegateRegister 
    ///KeyLongDelegateUnRegister KeyDownDelegateUnRegister KeyDownDelegateRegister


    ///API-No.58
    /// <summary>
    /// 输入设备检测的目标，优先级为Head/BTRight/BTLeft/GTRight/GTLeft/GGTRight/GGLeft
    /// </summary>
    public static GameObject Target {
        get {
            if(API_Module_InputSystem_Head.Head != null) {
                return API_Module_InputSystem_Head.HeadHitTarget;
            } else if(API_Module_InputSystem_BT3Dof.BTRight != null) {
                return API_Module_InputSystem_BT3Dof.BTHitTarget(API_Module_InputSystem_BT3Dof.BTType.Right);
            } else if(API_Module_InputSystem_BT3Dof.BTLeft != null) {
                return API_Module_InputSystem_BT3Dof.BTHitTarget(API_Module_InputSystem_BT3Dof.BTType.Left);
            }else if(API_Module_InputSystem_GGT26Dof.GGTRight != null) {
                return API_Module_InputSystem_GGT26Dof.GGTHitTarget(API_Module_InputSystem_GGT26Dof.GGestureType.Right);
            } else if(API_Module_InputSystem_GGT26Dof.GGTLeft != null) {
                return API_Module_InputSystem_GGT26Dof.GGTHitTarget(API_Module_InputSystem_GGT26Dof.GGestureType.Left);
            }
            return null;
        }
    }

    ///API-No.59
    /// <summary>
    /// 输入设备的发射射线起点，优先级为Head/BTRight/BTLeft/GTRight/GTLeft/GGTRight/GGLeft
    /// </summary>
    public static GameObject Gazer {
        get {
            if(API_Module_InputSystem_Head.Head != null) {
                return API_Module_InputSystem_Head.Head.detectorBase.pointerBase.gameObject;
            } else if(API_Module_InputSystem_BT3Dof.BTRight != null) {
                return API_Module_InputSystem_BT3Dof.BTRight.bT3DofDetector.pointerBase.gameObject;
            } else if(API_Module_InputSystem_BT3Dof.BTLeft != null) {
                return API_Module_InputSystem_BT3Dof.BTLeft.bT3DofDetector.pointerBase.gameObject;
            }else if(API_Module_InputSystem_GGT26Dof.GGTRight != null) {
                return API_Module_InputSystem_GGT26Dof.GGTRight.detectorBase.pointerBase.gameObject;
            } else if(API_Module_InputSystem_GGT26Dof.GGTLeft != null) {
                return API_Module_InputSystem_GGT26Dof.GGTLeft.detectorBase.pointerBase.gameObject;
            }
            return null;
        }
    }


    ///API-No.60
    /// <summary>
    /// 输入设备的发射射线方向，优先级为Head/BTRight/BTLeft/GTRight/GTLeft/GGTRight/GGLeft
    /// </summary>
    public static Vector3 Normal {
        get {
            if(Gazer!=null) {
                return Gazer.transform.forward;
            }
            return Vector3.zero;
        }
    }

    ///API-No.61
    /// <summary>
    /// 输入设备Cursor的位置，优先级为Head/BTRight/BTLeft/GTRight/GTLeft/GGTRight/GGLeft
    /// </summary>
    public static Vector3 Position {
        get {
            if(API_Module_InputSystem_Head.Head != null) {
                return API_Module_InputSystem_Head.GetHeadCursor.transform.position;
            } else if(API_Module_InputSystem_BT3Dof.BTRight != null) {
                return API_Module_InputSystem_BT3Dof.GetBTCursor(API_Module_InputSystem_BT3Dof.BTType.Right).transform.position;
            } else if(API_Module_InputSystem_BT3Dof.BTLeft != null) {
                return API_Module_InputSystem_BT3Dof.GetBTCursor(API_Module_InputSystem_BT3Dof.BTType.Left).transform.position;
            }else if(API_Module_InputSystem_GGT26Dof.GGTRight != null) {
                return API_Module_InputSystem_GGT26Dof.GetGGTCursor(API_Module_InputSystem_GGT26Dof.GGestureType.Right).transform.position;
            } else if(API_Module_InputSystem_GGT26Dof.GGTLeft != null) {
                return API_Module_InputSystem_GGT26Dof.GetGGTCursor(API_Module_InputSystem_GGT26Dof.GGestureType.Left).transform.position;
            }
            return Vector3.zero;
        }
    }

    ///API-No.62
    /// <summary>
    /// 获取当前的具体输入设备，优先级为Head/BTRight/BTLeft/GTRight/GTLeft/GGTRight/GGLeft
    /// </summary>
    public static InputDevicePartBase InputDeviceCurrent {
        get {
            if(API_Module_InputSystem_Head.Head != null) {
                return API_Module_InputSystem_Head.Head;
            } else if(API_Module_InputSystem_BT3Dof.BTRight != null) {
                return API_Module_InputSystem_BT3Dof.BTRight;
            } else if(API_Module_InputSystem_BT3Dof.BTLeft != null) {
                return API_Module_InputSystem_BT3Dof.BTLeft;
            }
            return null;
        }
    }

    public static bool InputDeviceStatus(InputDeviceType deviceType) {
        if (Module_InputSystem.instance) {
            return Module_InputSystem.instance.GetInputDeviceStatus(deviceType);
        }
        return false;
    }

}
