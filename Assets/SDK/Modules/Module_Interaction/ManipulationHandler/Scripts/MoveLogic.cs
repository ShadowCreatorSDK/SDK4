using SC.XR.Unity.Module_InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic
{
    private float pointerRefDistance;

    private bool pointerPosIndependentOfHead = true;

    private Vector3 pointerLocalGrabPoint;
    private Vector3 objectLocalGrabPoint;
    private Vector3 grabToObject;

    /// <summary>
    /// Setup function
    /// </summary>
    public void Setup(SCPose pointerCentroidPose, Vector3 grabCentroid, Transform objectPose, Vector3 objectScale)
    {
        pointerRefDistance = GetDistanceToBody(pointerCentroidPose);

        pointerPosIndependentOfHead = pointerRefDistance != 0;

        Quaternion worldToPointerRotation = Quaternion.Inverse(pointerCentroidPose.rotation);
        pointerLocalGrabPoint = worldToPointerRotation * (grabCentroid - pointerCentroidPose.position);

        objectLocalGrabPoint = Quaternion.Inverse(objectPose.rotation) * (grabCentroid - objectPose.position);
        objectLocalGrabPoint = Vector3.Scale(objectLocalGrabPoint, new Vector3(1f / objectScale.x, 1f / objectScale.y, 1f / objectScale.z)) ;

        grabToObject = objectPose.position - grabCentroid;
    }

    /// <summary>
    /// Update the position based on input.
    /// </summary>
    /// <returns>A Vector3 describing the desired position</returns>
    public Vector3 Update(SCPose pointerCentroidPose, Quaternion objectRotation, Vector3 objectScale, bool usePointerRotation)
    {
        float distanceRatio = 1.0f;

        if (pointerPosIndependentOfHead)
        {
            // Compute how far away the object should be based on the ratio of the current to original hand distance
            float currentHandDistance = GetDistanceToBody(pointerCentroidPose);
            distanceRatio = currentHandDistance / pointerRefDistance;
        }

        if (usePointerRotation)
        {
            Vector3 scaledGrabToObject = Vector3.Scale(objectLocalGrabPoint, objectScale);
            Vector3 adjustedPointerToGrab = (pointerLocalGrabPoint * distanceRatio);
            adjustedPointerToGrab = pointerCentroidPose.rotation * adjustedPointerToGrab;

            return adjustedPointerToGrab - objectRotation * scaledGrabToObject + pointerCentroidPose.position;
        }
        else
        {
            return pointerCentroidPose.position + (pointerCentroidPose.rotation * pointerLocalGrabPoint + grabToObject) * distanceRatio;
        }
    }

    private float GetDistanceToBody(SCPose pointerCentroidPose)
    {
        // The body is treated as a ray, parallel to the y-axis, where the start is head position.
        // This means that moving your hand down such that is the same distance from the body will
        // not cause the manipulated object to move further away from your hand. However, when you
        // move your hand upward, away from your head, the manipulated object will be pushed away.
        if (pointerCentroidPose.position.y > SvrManager.Instance.head.transform.position.y)
        {
            return Vector3.Distance(pointerCentroidPose.position, SvrManager.Instance.head.transform.position);
        }
        else
        {
            Vector2 headPosXZ = new Vector2(SvrManager.Instance.head.transform.position.x, SvrManager.Instance.head.transform.position.z);
            Vector2 pointerPosXZ = new Vector2(pointerCentroidPose.position.x, pointerCentroidPose.position.z);

            return Vector2.Distance(pointerPosXZ, headPosXZ);
        }
    }
}
