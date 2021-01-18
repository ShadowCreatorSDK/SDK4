using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OneDevicePartCountManipulation:单个设备交互(头,单手柄,单手)
/// TwoDevicePartCountManipulation:两个设备交互(双手柄,双手)
/// </summary>

public interface IDevicePartCountManipulation
{
    void Init(IDevicePartManipulation devicePartManipulation, Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic);
    Tuple<Vector3, Quaternion, Vector3> Update(Func<Vector3, Vector3> scaleConstraint);
}
