using System;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceHead;
using UnityEngine.EventSystems;

public class API_Module_InputSystem_Head
{

    ///API-No.100
    /// <summary>
    /// 获取InputSystem支持的头部输入设备,头部输入设备包含一个Part，名曰：Head
    /// </summary>
    /// <returns>null表示不支持或者InputSystem未初始好</returns>
    public static InputDeviceHead HeadDevice {
        get {
            if(Module_InputSystem.instance) {
                return Module_InputSystem.instance.GetInputDevice<InputDeviceHead>(InputDeviceType.Head);
            }
            return null;
        }
    }

    ///API-No.101
    /// <summary>
    /// 头部输入设备包含的Part
    /// </summary>
    public static InputDeviceHeadPart Head {
        get {
            if(HeadDevice && HeadDevice.inputDevicePartList[0]) {
                //return HeadDevice.inputDeviceHeadPartList[0].inputDataBase.isVaild ? HeadDevice.inputDeviceHeadPartList[0] : null;
                return HeadDevice.inputDevicePartList[0] as InputDeviceHeadPart;
            }
            return null;
        }
    }

    ///API-No.102
    /// <summary>
    /// Head的四元数，全局坐标
    /// </summary>
    public static Quaternion HeadRotation {
        get {
            if(Head) {
                return Head.inputDataHead.rotation;
            }
            return Quaternion.identity;            
        }
    }

    ///API-No.103
    /// <summary>
    /// Head的位置信息，全局坐标
    /// </summary>
    public static Vector3 HeadPosition {
        get {
            if(Head) {
                return Head.inputDataHead.position;
            }
            return Vector3.zero;
        }
    }

    ///API-No.104
    /// <summary>
    /// Head的检测到碰撞信息集合，包含碰撞到的物体，数据等
    /// </summary>
    public static SCPointEventData HeadPointerEventData {
        get {
            if(Head) {
                return Head.inputDataHead.SCPointEventData;
            }
            return null;
        }
    }

    ///API-No.105
    /// <summary>
    /// Head碰撞到Collider，若不为null,可以通过HeadHitInfo获取碰撞信息，
    /// </summary>
    public static GameObject HeadHitTarget {
        get {
            if(HeadPointerEventData != null) {
                return HeadPointerEventData.pointerCurrentRaycast.gameObject;
            }
            return null;
        }
    }

    ///API-No.106
    /// <summary>
    /// Head碰撞信息
    /// </summary>
    /// <returns></returns>
    public static RaycastResult HeadHitInfo {
        get{
            if(HeadPointerEventData != null) {
                return HeadPointerEventData.pointerCurrentRaycast;
            }
            return new RaycastResult();
        }
    }

    ///API-No.107
    /// <summary>
    /// Head拖拽的物体
    /// </summary>
    public static GameObject HeadDragTarget {
        get {
            if(HeadPointerEventData != null) {
                return HeadPointerEventData.pointerDrag;
            }
            return null;
        }
    }

    ///API-No.108
    /// <summary>
    /// Head某个按键是否按下，当前帧有效，下帧复位，参考Input.GetKeyDown
    /// </summary>
    /// <param name="inputKeyCode">具体按键，Head支持Enter/Back</param>
    /// <returns></returns>
    public static bool IsHeadKeyDown(InputKeyCode inputKeyCode) {
        if(Head) {
            return Head.inputDataHead.inputKeys.GetKeyDown(inputKeyCode);
        }
        return false;
    }

    ///API-No.109
    /// <summary>
    /// Head某个按键是否按下后松开，当前帧有效，下帧复位，参考Input.GetKeyUp
    /// </summary>
    /// <param name="inputKeyCode">具体按键，Head支持Enter/Back</param>
    /// <returns></returns>
    public static bool IsHeadKeyUp(InputKeyCode inputKeyCode) {
        if(Head) {
            return Head.inputDataHead.inputKeys.GetKeyUp(inputKeyCode);
        }
        return false;
    }

    ///API-No.110
    /// <summary>
    /// Head某个按键的状态信息，当前帧有效，下帧复位
    /// </summary>
    /// <param name="inputKeyCode">具体按键，Head支持Enter/Back</param>
    /// <returns></returns>
    public static InputKeyState HeadKeyState(InputKeyCode inputKeyCode) {
        if(Head) {
            return Head.inputDataHead.inputKeys.GetKeyState(inputKeyCode);
        }
        return InputKeyState.Null;
    }

    ///API-No.111
    /// <summary>
    /// Head某个按键的实时状态，参考Input.GetKey
    /// </summary>
    /// <param name="inputKeyCode">具体按键，Head支持Enter/Back</param>
    /// <returns></returns>
    public static InputKeyState HeadKeyCurrentState(InputKeyCode inputKeyCode) {
        if(Head) {
            return Head.inputDataHead.inputKeys.GetKeyCurrentState(inputKeyCode);
        }
        return InputKeyState.Null;
    }

    ///API-No.112
    /// <summary>
    /// 给Head发送一个按键，注意，发送按键至少需发送一个Down，然后再发送一个Up，此API模拟按键按下动作
    /// </summary>
    /// <param name="inputKeyCode">具体按键</param>
    /// <param name="inputKeyState">按键的状态</param>
    public static void HeadAddKey(InputKeyCode inputKeyCode, InputKeyState inputKeyState) {
        if(Head) {
            Head.inputDataHead.inputKeys.InputDataAddKey(inputKeyCode, inputKeyState);
        }
    }

    ///API-No.113
    /// <summary>
    /// 设置可检测Collider的范围半径 默认50米
    /// </summary>
    /// <param name="distance">单位米</param>
    public static void SetHeadRayCastDistance(float distance) {
        if(Head && distance > 0) {         
            Head.detectorBase.pointerBase.MaxDetectDistance = distance;
        }
    }

    ///API-No.114
    /// <summary>
    /// 设置可见Cursor的长度 默认3米
    /// </summary>
    /// <param name="distance">单位米</param>
    public static void SetHeadEndPointerDistance(float distance) {
        if(Head && distance > 0) {
            GetHeadCursor.DefaultDistance = distance;
        }
    }

    ///API-No.115
    /// <summary>
    /// 获取Head光束终点的Curosor对象
    /// </summary>
    public static DefaultCursor GetHeadCursor {
        get {
            if(Head) {
                return Head.detectorBase.pointerBase.cursorBase as DefaultCursor;
            }
            return null;
        }
    }
}
