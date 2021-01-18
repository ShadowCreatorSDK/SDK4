using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDevicePartCountManipulation : IDevicePartCountManipulation
{
    protected IDevicePartManipulation devicePartManipulation;

    public virtual void Init(IDevicePartManipulation devicePartManipulation, Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        this.devicePartManipulation = devicePartManipulation;
    }

    public abstract Tuple<Vector3, Quaternion, Vector3> Update(Func<Vector3, Vector3> scaleConstraint);
}