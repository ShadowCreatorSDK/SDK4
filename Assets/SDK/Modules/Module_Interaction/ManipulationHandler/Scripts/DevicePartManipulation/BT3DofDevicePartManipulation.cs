using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Obsolete()]
public class BT3DofDevicePartManipulation : AbstractDevicePartManipulation
{
    private SCPointEventData eventData;
    private SCPose pointerPose;

    public override void OneDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        base.OneDevicePartInit(eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);

        this.eventData = this.eventDataDic.Values.ToArray()[0];
        pointerPose = new SCPose(Vector3.zero, Quaternion.identity);
        Vector3 grabPosition = Vector3.zero;
        pointerPose.position = this.eventData.Position3D;
        pointerPose.rotation = Quaternion.identity;
        grabPosition = this.eventData.Position3D;

        //Head only need to setup movelogic
        moveLogic.Setup(pointerPose, grabPosition, targetTransform, targetTransform.localScale);
    }

    public override Tuple<Vector3, Quaternion, Vector3> OneDevicePartUpdate()
    {
        pointerPose.position = this.eventData.Position3D;
        pointerPose.rotation = Quaternion.identity;

        Vector3 position = moveLogic.Update(pointerPose, targetTransform.rotation, targetTransform.localScale, true);

        //TODO
        return new Tuple<Vector3, Quaternion, Vector3>(position, targetTransform.rotation, targetTransform.localScale);
    }

    public override void TwoDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        base.TwoDevicePartInit(eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);
        //TODO
    }

    public override Tuple<Vector3, Quaternion, Vector3> TwoDevicePartUpdate(Func<Vector3, Vector3> scaleConstraint)
    {
        //TODO
        return new Tuple<Vector3, Quaternion, Vector3>(Vector3.zero, Quaternion.identity, Vector3.one);
    }
}
