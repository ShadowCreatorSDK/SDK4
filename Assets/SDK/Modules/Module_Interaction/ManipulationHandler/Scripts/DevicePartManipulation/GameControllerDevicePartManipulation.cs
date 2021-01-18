using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControllerDevicePartManipulation : AbstractDevicePartManipulation
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

        moveLogic.Setup(pointerPose, grabPosition, targetTransform, targetTransform.localScale);
    }

    public override Tuple<Vector3, Quaternion, Vector3> OneDevicePartUpdate()
    {
        pointerPose.position = this.eventData.Position3D;
        pointerPose.rotation = Quaternion.identity;

        Vector3 position = moveLogic.Update(pointerPose, targetTransform.rotation, targetTransform.localScale, true); ;

        return new Tuple<Vector3, Quaternion, Vector3>(position, targetTransform.rotation, targetTransform.localScale);
    }

    private Dictionary<InputDevicePartType, Vector3> position3DDic;
    private SCPose[] position3DPoses = new SCPose[2];

    public override void TwoDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        base.TwoDevicePartInit(eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);

        if (position3DDic == null)
        {
            position3DDic = new Dictionary<InputDevicePartType, Vector3>();
        }
        position3DDic.Clear();

        foreach (var eventData in eventDataDic)
        {
            position3DDic.Add(eventData.Key, eventData.Value.Position3D);
        }

        if (scaleLogic != null)
        {
            scaleLogic.Setup(eventDataDic.Values.Select(item => item.Position3D).ToArray(), targetTransform);
        }

        if (rotateLogic != null)
        {
            rotateLogic.Setup(eventDataDic.Values.Select(item => item.Position3D).ToArray(), targetTransform);
        }

        int count = 0;
        foreach (SCPointEventData eventDataItem in eventDataDic.Values)
        {
            position3DPoses[count] = new SCPose(eventDataItem.Position3D, Quaternion.identity);
            count++;
        }
        SCPose pointerCentroidPose = GetHandTipPointCentroid(position3DPoses);
        Vector3 grabCentroid = GetRaycastPointCentroid(position3DDic.Values.ToArray());

        moveLogic.Setup(pointerCentroidPose, grabCentroid, targetTransform, targetTransform.localScale);
    }

    public override Tuple<Vector3, Quaternion, Vector3> TwoDevicePartUpdate(Func<Vector3, Vector3> scaleConstraint)
    {
        Vector3 scale = targetTransform.localScale;
        if (scaleLogic != null)
        {
            scale = scaleLogic.UpdateMap(eventDataDic.Values.Select(item => item.Position3D).ToArray());
        }
        scale = scaleConstraint(scale);

        Quaternion rotation = targetTransform.rotation;
        if (rotateLogic != null)
        {
            rotation = rotateLogic.Update(eventDataDic.Values.Select(item => item.Position3D).ToArray(), rotation);
        }

        int count = 0;
        foreach (SCPointEventData eventDataItem in eventDataDic.Values)
        {
            position3DPoses[count].position = eventDataItem.Position3D;
            position3DPoses[count].rotation = Quaternion.identity;//handTipTransformArray[count].rotation;
            count++;
        }

        SCPose pointerCentroidPose = GetHandTipPointCentroid(position3DPoses);

        Vector3 position = moveLogic.Update(pointerCentroidPose, rotation, scale, true);

        return new Tuple<Vector3, Quaternion, Vector3>(position, rotation, scale);
    }

    private SCPose GetHandTipPointCentroid(SCPose[] tipPoints)
    {
        Vector3 sumPos = Vector3.zero;
        Vector3 sumDir = Vector3.zero;
        int count = tipPoints.Length;
        for (int i = 0; i < count; i++)
        {
            sumPos += tipPoints[i].position;
            sumDir += tipPoints[i].rotation * Vector3.forward;
        }

        Vector3 resultPos = sumPos / Math.Max(1, count);
        Quaternion resultRot = Quaternion.LookRotation(sumDir / Math.Max(1, count));

        return new SCPose(resultPos, resultRot);
    }

    private Vector3 GetRaycastPointCentroid(Vector3[] raycastPoints)
    {
        Vector3 sum = Vector3.zero;
        int count = raycastPoints.Length;
        for (int i = 0; i < count; i++)
        {
            sum += raycastPoints[i];
        }
        return sum / Math.Max(1, count);
    }
}
