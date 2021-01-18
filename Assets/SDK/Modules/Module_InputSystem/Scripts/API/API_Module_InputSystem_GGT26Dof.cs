using System;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof;
using UnityEngine.EventSystems;

public class API_Module_InputSystem_GGT26Dof
{

    ///API-No.250
    /// <summary>
    /// 获取InputSystem支持的灰度手势输入设备,灰度手势输入设备包含二个Part，名曰：GGTLeft,GGTRight,也就是左手/右手
    /// </summary>
    /// <returns>null表示不支持或者Module_InputSystem未初始好</returns>
    public static InputDeviceHand GGT26DofDevice
    {
        get {
            if(Module_InputSystem.instance) {
                return Module_InputSystem.instance.GetInputDevice<InputDeviceHand>(InputDeviceType.GGT26Dof);
            }
            return null;
        }
    }

    public enum GGestureType {
        Left=0,
        Right=1,
    }

    ///API-No.251
    /// <summary>
    /// 灰度手势输入设备左手
    /// </summary>
    public static InputDeviceGGT26DofPart GGTLeft {
        get {
            if(GGT26DofDevice && GGT26DofDevice.inputDevicePartList[0]) {
                //return Gesture26Dof.InputDevice26DofGesturePartList[0].inputDataBase.isVaild ? Gesture26Dof.InputDevice26DofGesturePartList[0] : null;
                return GGT26DofDevice.inputDevicePartList[0]as InputDeviceGGT26DofPart;
            }
            return null;
        }
    }

    ///API-No.252
    /// <summary>
    /// 灰度手势输入设备右手
    /// </summary>
    public static InputDeviceGGT26DofPart GGTRight {
        get {
            if(GGT26DofDevice && GGT26DofDevice.inputDevicePartList[1]) {
                //return Gesture26Dof.InputDevice26DofGesturePartList[1].inputDataBase.isVaild ? Gesture26Dof.InputDevice26DofGesturePartList[1] : null;
                return GGT26DofDevice.inputDevicePartList[1] as InputDeviceGGT26DofPart;
            }
            return null;
        }
    }

    ///API-No.253
    /// <summary>
    /// GGTLeft/GGTRight的手势数据，具体数据见handInfo
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static handInfo GGThandInfo(GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDataGGT26Dof.handInfo;
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDataGGT26Dof.handInfo;
        }
        return null;
    }

    ///API-No.254
    /// <summary>
    /// GGTLeft/GGTRight 检测到碰撞信息集合，包含碰撞到的物体，数据等
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static SCPointEventData GGTPointerEventData(GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDataGGT26Dof.SCPointEventData;
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDataGGT26Dof.SCPointEventData;
        }
        return null;
    }

    ///API-No.255
    /// <summary>
    ///GGTLeft/GGTRight 碰撞到Collider，若不为null,可以通过GTHitInfo获取碰撞信息，
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static GameObject GGTHitTarget(GGestureType type = GGestureType.Right) {
        if(GGTPointerEventData(type) != null) {
            return GGTPointerEventData(type).pointerCurrentRaycast.gameObject;
        }
        return null;
    }

    ///API-No.256
    /// <summary>
    /// GGTLeft/GGTRight 碰撞信息
    /// </summary>
    /// <returns></returns>
    public static RaycastResult GGTHitInfo(GGestureType type = GGestureType.Right) {
        if(GGTPointerEventData(type) != null) {
            return GGTPointerEventData(type).pointerCurrentRaycast;
        }
        return new RaycastResult();
    }

    ///API-No.257
    /// <summary>
    /// GGTLeft/GGTRight 拖拽的物体
    /// </summary>
    public static GameObject GGTDragTarget(GGestureType type = GGestureType.Right) {
        if(GGTPointerEventData(type) != null) {
            return GGTPointerEventData(type).pointerDrag;
        }
        return null;
    }

    ///API-No.258
    /// <summary>
    /// GGTLeft/GGTRight某个按键是否按下，当前帧有效，下帧复位，参考Input.GetKeyDown
    /// </summary>
    /// <param name="inputKeyCode">具体按键，GGTLeft/GGTRight支持Enter/Back/Function</param>
    /// <returns></returns>
    public static bool IsGGTKeyDown(InputKeyCode inputKeyCode, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDataGGT26Dof.inputKeys.GetKeyDown(inputKeyCode);
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDataGGT26Dof.inputKeys.GetKeyDown(inputKeyCode);
        }
        return false;
    }

    ///API-No.259
    /// <summary>
    /// GGTLeft/GGTRight某个按键是否按下后松开，当前帧有效，下帧复位，参考Input.GetKeyUp
    /// </summary>
    /// <param name="inputKeyCode">具体按键，GGTLeft/GGTRight支持Enter/Back/Function</param>
    /// <returns></returns>
    public static bool IsGGTKeyUp(InputKeyCode inputKeyCode, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDataGGT26Dof.inputKeys.GetKeyUp(inputKeyCode);
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDataGGT26Dof.inputKeys.GetKeyUp(inputKeyCode);
        }
        return false;
    }

    ///API-No.260
    /// <summary>
    /// GGTLeft/GGTRight某个按键的状态信息，当前帧有效，下帧复位
    /// </summary>
    /// <param name="inputKeyCode">具体按键，GGTLeft/GGTRight支持Enter/Back/Function</param>
    /// <returns></returns>
    public static InputKeyState GGTKeyState(InputKeyCode inputKeyCode, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDataGGT26Dof.inputKeys.GetKeyState(inputKeyCode);
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDataGGT26Dof.inputKeys.GetKeyState(inputKeyCode);
        }
        return InputKeyState.Null;
    }

    ///API-No.261
    /// <summary>
    /// GGTLeft/GGTRight某个按键的实时状态，参考Input.GetKey
    /// </summary>
    /// <param name="inputKeyCode">具体按键，GGTLeft/GGTRight支持Enter/Back/Function</param>
    /// <returns></returns>
    public static InputKeyState GGTKeyCurrentState(InputKeyCode inputKeyCode, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDataGGT26Dof.inputKeys.GetKeyCurrentState(inputKeyCode);
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDataGGT26Dof.inputKeys.GetKeyCurrentState(inputKeyCode);
        }
        return InputKeyState.Null;
    }

    ///API-No.262
    /// <summary>
    /// GGTLeft/GGTRight发送一个按键，注意，发送按键至少需发送一个Down，然后再发送一个Up，此API模拟按键按下动作
    /// </summary>
    /// <param name="inputKeyCode">具体按键</param>
    /// <param name="inputKeyState">按键的状态</param>
    public static void GGTKeyAddKey(InputKeyCode inputKeyCode, InputKeyState inputKeyState, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            GGTLeft.inputDataGGT26Dof.inputKeys.InputDataAddKey(inputKeyCode, inputKeyState);
        } else if(GGTRight && type == GGestureType.Right) {
            GGTRight.inputDataGGT26Dof.inputKeys.InputDataAddKey(inputKeyCode, inputKeyState);
        }
    }

    ///API-No.263
    /// <summary>
    /// GGTLeft/GGTRight设置可检测Collider的范围半径 默认30米
    /// </summary>
    /// <param name="distance">单位米</param>
    public static void SetGGTRayCastDistance(float distance, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            GGTLeft.detectorBase.pointerBase.MaxDetectDistance = distance;
        } else if(GGTRight && type == GGestureType.Right) {
            GGTRight.detectorBase.pointerBase.MaxDetectDistance = distance;
        }
    }

    ///API-No.264
    /// <summary>
    /// GGTLeft/GGTRight设置可见光束的长度 默认3米
    /// </summary>
    /// <param name="distance">单位米</param>
    public static void SetGGTEndPointerDistance(float distance, GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            GetGGTCursor(type).DefaultDistance = distance;
        } else if(GGTRight && type == GGestureType.Right) {
            GetGGTCursor(type).DefaultDistance = distance;
        }
    }

    ///API-No.265
    /// <summary>
    /// GGTLeft/GGTRight获取光束终点的Cursor对象
    /// </summary>
    /// <param name="distance">单位米</param>
    public static DefaultCursor GetGGTCursor(GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.detectorBase.pointerBase.cursorBase as DefaultCursor;
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.detectorBase.pointerBase.cursorBase as DefaultCursor;
        }
        return null;
    }

    ///API-No.266
    /// <summary>
    /// GGTLeft/GGTRight获取光束
    /// </summary>
    /// <param name="distance">单位米</param>
    public static LineRenderer GetGGTLine(GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.detectorBase.pointerBase.lineBase.lineRenderer;
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.detectorBase.pointerBase.lineBase.lineRenderer;
        }
        return null;
    }

    ///API-No.267
    /// <summary>
    /// 开启GGTLeft
    /// </summary>
    /// <param name="type"></param>
    public static void EnableGGT(GGestureType type = GGestureType.Right) {
        if(GGT26DofDevice && type == GGestureType.Left) {
            GGT26DofDevice.LeftHandOpen = true;
        } else if(GGT26DofDevice && type == GGestureType.Right) {
            GGT26DofDevice.RightHandOpen = true;
        }
    }

    ///API-No.268
    /// <summary>
    /// 关闭GGTLeft
    /// </summary>
    /// <param name="type"></param>
    public static void DisableableGGT(GGestureType type = GGestureType.Right) {
        if(GGT26DofDevice && type == GGestureType.Left) {
            GGT26DofDevice.LeftHandOpen = false;
        } else if(GGT26DofDevice && type == GGestureType.Right) {
            GGT26DofDevice.RightHandOpen = false;
        }
    }

    ///API-No.269
    /// <summary>
    /// 获取手势游戏对象，具体见fingerUI
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [Obsolete("Use  instead")]
    public static FingerUI[] GetFingerUI(GGestureType type = GGestureType.Right) {
        if(GGTLeft && type == GGestureType.Left) {
            return GGTLeft.inputDeviceGGT26DofPartUI.modelGGT26Dof.fingerUI;
        } else if(GGTRight && type == GGestureType.Right) {
            return GGTRight.inputDeviceGGT26DofPartUI.modelGGT26Dof.fingerUI;
        }
        return null;
    }

   
}
