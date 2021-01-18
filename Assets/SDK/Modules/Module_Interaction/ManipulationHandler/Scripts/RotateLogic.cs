using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLogic
{
    private Vector3 startHandlebar;
    private Quaternion startRotation;

    /// <summary>
    /// Setup the rotation logic.
    /// </summary>
    /// <param name="handsPressedArray">Array with positions of down pointers</param>
    public void Setup(Transform[] handsPressedArray, Transform t)
    {
        startHandlebar = GetHandlebarDirection(handsPressedArray);
        startRotation = t.rotation;
    }

    public void Setup(Vector3[] handsPressedArray, Transform t)
    {
        startHandlebar = GetHandlebarDirection(handsPressedArray);
        startRotation = t.rotation;
    }

    /// <summary>
    /// Update the rotation based on input.
    /// </summary>
    /// <param name="handsPressedArray">Array with positions of down pointers, order should be the same as handsPressedArray provided in Setup</param>
    /// <returns>Desired rotation</returns>
    public Quaternion Update(Transform[] handsPressedArray, Quaternion currentRotation)
    {
        var handlebarDirection = GetHandlebarDirection(handsPressedArray);
        return Quaternion.FromToRotation(startHandlebar, handlebarDirection) * startRotation;
    }

    public Quaternion Update(Vector3[] handsPressedArray, Quaternion currentRotation)
    {
        var handlebarDirection = GetHandlebarDirection(handsPressedArray);
        return Quaternion.FromToRotation(startHandlebar, handlebarDirection) * startRotation;
    }

    private static Vector3 GetHandlebarDirection(Transform[] handsPressedArray)
    {
        Debug.Assert(handsPressedArray.Length > 1);
        return handsPressedArray[1].position - handsPressedArray[0].position;
    }

    private static Vector3 GetHandlebarDirection(Vector3[] handsPressedArray)
    {
        Debug.Assert(handsPressedArray.Length > 1);
        return handsPressedArray[1] - handsPressedArray[0];
    }
}
