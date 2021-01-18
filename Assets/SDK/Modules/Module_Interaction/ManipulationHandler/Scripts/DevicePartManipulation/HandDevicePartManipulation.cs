using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandDevicePartManipulation : AbstractDevicePartManipulation
{
    //for one hand
    private Quaternion startRotation;
    private Quaternion localRotationInHand;

    private SCPose oneHandPose = new SCPose(Vector3.zero, Quaternion.identity);
    private Vector3 oneHandGrabPosition;
    private Transform oneHandJointFour;
    private HandDetector handDetector;

    private SCPointEventData onHandPointEventData;

    public override void OneDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        base.OneDevicePartInit(eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);
        startRotation = targetTransform.rotation;
        onHandPointEventData = eventDataDic.Values.ToArray()[0];

        InputDeviceHandPart inputDeviceHandPart = onHandPointEventData.inputDevicePartBase as InputDeviceHandPart;
        ModelHand modelHand = inputDeviceHandPart.inputDeviceHandPartUI.modelHand;
        oneHandJointFour = modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).transform;
        localRotationInHand = Quaternion.Inverse(oneHandJointFour.rotation) * startRotation;

        handDetector = inputDeviceHandPart.detectorBase as HandDetector;

        oneHandPose.position = onHandPointEventData.Position3D;
        oneHandPose.rotation = startRotation;
        oneHandGrabPosition = onHandPointEventData.Position3D;

        moveLogic.Setup(oneHandPose, oneHandGrabPosition, targetTransform, targetTransform.localScale);
    }

    public override Tuple<Vector3, Quaternion, Vector3> OneDevicePartUpdate()
    {
        Quaternion rotation = targetTransform.rotation;
        if (handDetector.currentPointer is INearPointer && rotateLogic != null)
        {
            rotation = oneHandJointFour.rotation * localRotationInHand;
        }

        oneHandPose.position = onHandPointEventData.Position3D;
        oneHandPose.rotation = startRotation;

        Vector3 position = moveLogic.Update(oneHandPose, rotation, targetTransform.localScale, true);

        return new Tuple<Vector3, Quaternion, Vector3>(position, rotation, targetTransform.localScale);
    }

    ///for two hand
    private Dictionary<InputDevicePartType, Transform> handTipTransformDic;
    private Dictionary<InputDevicePartType, Vector3> position3DDic;
    private SCPose[] position3DPoses = new SCPose[2];
    private Transform[] handTipTransformArray;

    public override void TwoDevicePartInit(Dictionary<InputDevicePartType, SCPointEventData> eventDataDic, Transform targetTransform, MoveLogic moveLogic, RotateLogic rotateLogic, ScaleLogic scaleLogic)
    {
        base.TwoDevicePartInit(eventDataDic, targetTransform, moveLogic, rotateLogic, scaleLogic);

        if (handTipTransformDic == null)
        {
            handTipTransformDic = new Dictionary<InputDevicePartType, Transform>();
        }
        handTipTransformDic.Clear();

        if (position3DDic == null)
        {
            position3DDic = new Dictionary<InputDevicePartType, Vector3>();
        }
        position3DDic.Clear();

        foreach (var eventData in eventDataDic)
        {
            InputDeviceHandPart inputDeviceHandPart = eventData.Value.inputDevicePartBase as InputDeviceHandPart;
            ModelHand modelHand = inputDeviceHandPart.inputDeviceHandPartUI.modelHand;
            Transform tipTransform = modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.One).transform;
            handTipTransformDic.Add(eventData.Key, tipTransform);
            position3DDic.Add(eventData.Key, eventData.Value.Position3D);
        }

        handTipTransformArray = handTipTransformDic.Values.ToArray();

        if (scaleLogic != null)
        {
            scaleLogic.Setup(handTipTransformArray, targetTransform);
        }

        if (rotateLogic != null)
        {
            rotateLogic.Setup(handTipTransformArray, targetTransform);
        }

        int count = 0;
        foreach (SCPointEventData eventDataItem in eventDataDic.Values)
        {
            position3DPoses[count] = new SCPose(eventDataItem.Position3D, handTipTransformArray[count].rotation);
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
            scale = scaleLogic.UpdateMap(handTipTransformArray);
        }
        scale = scaleConstraint(scale);

        Quaternion rotation = targetTransform.rotation;
        if (rotateLogic != null)
        {
            rotation = rotateLogic.Update(handTipTransformArray, rotation);
        }

        int count = 0;
        foreach (SCPointEventData eventDataItem in eventDataDic.Values)
        {
            position3DPoses[count].position = eventDataItem.Position3D;
            position3DPoses[count].rotation = handTipTransformArray[count].rotation;
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
