using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDevicePartManipulation : IDevicePartManipulation
{
    protected Dictionary<InputDevicePartType, SCPointEventData> eventDataDic;
    protected Transform targetTransform;
    protected MoveLogic moveLogic;
    protected RotateLogic rotateLogic;
    protected ScaleLogic scaleLogic;

    public virtual void OneDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        this.eventDataDic = eventDataDic;
        this.targetTransform = targetTransform;
        this.moveLogic = moveLogic;
        this.rotateLogic = rotateLogic;
        this.scaleLogic = scaleLogic;
    }

    public abstract Tuple<Vector3, Quaternion, Vector3> OneDevicePartUpdate();

    public virtual void TwoDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        this.eventDataDic = eventDataDic;
        this.targetTransform = targetTransform;
        this.moveLogic = moveLogic;
        this.rotateLogic = rotateLogic;
        this.scaleLogic = scaleLogic;
    }

    public abstract Tuple<Vector3, Quaternion, Vector3> TwoDevicePartUpdate(Func<Vector3, Vector3> scaleConstraint);
}
