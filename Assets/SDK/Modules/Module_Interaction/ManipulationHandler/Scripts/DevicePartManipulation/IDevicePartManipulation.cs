using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HeadDevicePartManipulation 头部操作
/// BT3DofDevicePartManipulation 手柄操作
/// HandDevicePartManipulation 手势操作
/// </summary>
public interface IDevicePartManipulation
{
    void OneDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic);
    Tuple<Vector3, Quaternion, Vector3> OneDevicePartUpdate();
    void TwoDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic);
    Tuple<Vector3, Quaternion, Vector3> TwoDevicePartUpdate(Func<Vector3, Vector3> scaleConstraint);
}
