using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDevicePartCountManipulation : AbstractDevicePartCountManipulation
{
    public override void Init(IDevicePartManipulation devicePartManipulation, Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        base.Init(devicePartManipulation, eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);
        this.devicePartManipulation.TwoDevicePartInit(eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);
    }

    public override Tuple<Vector3, Quaternion, Vector3> Update(Func<Vector3, Vector3> scaleConstraint)
    {
        return devicePartManipulation.TwoDevicePartUpdate(scaleConstraint);
    }
}
