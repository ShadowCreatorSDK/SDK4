using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLogic 
{
    private Vector3 startObjectScale;
    private float startHandDistanceMeters;

    /// <summary>
    /// Initialize system with source info from controllers/hands
    /// </summary>
    /// <param name="handsPressedArray">Array with positions of down pointers</param>
    /// <param name="manipulationRoot">Transform of gameObject to be manipulated</param>
    public virtual void Setup(Transform[] handsPressedArray, Transform manipulationRoot)
    {
        startHandDistanceMeters = GetMinDistanceBetweenHands(handsPressedArray);
        startObjectScale = manipulationRoot.transform.localScale;
    }

    public virtual void Setup(Vector3[] handsPressedArray, Transform manipulationRoot)
    {
        startHandDistanceMeters = GetMinDistanceBetweenHands(handsPressedArray);
        startObjectScale = manipulationRoot.transform.localScale;
    }

    /// <summary>
    /// update GameObject with new Scale state
    /// </summary>
    /// <param name="handsPressedArray">Array with positions of down pointers, order should be the same as handsPressedArray provided in Setup</param>
    /// <returns>a Vector3 describing the new Scale of the object being manipulated</returns>
    public virtual Vector3 UpdateMap(Transform[] handsPressedArray)
    {
        var ratioMultiplier = GetMinDistanceBetweenHands(handsPressedArray) / startHandDistanceMeters;
        return startObjectScale * ratioMultiplier;
    }

    public virtual Vector3 UpdateMap(Vector3[] handsPressedArray)
    {
        var ratioMultiplier = GetMinDistanceBetweenHands(handsPressedArray) / startHandDistanceMeters;
        return startObjectScale * ratioMultiplier;
    }

    private float GetMinDistanceBetweenHands(Transform[] handsPressedArray)
    {
        var result = float.MaxValue;
        for (int i = 0; i < handsPressedArray.Length; i++)
        {
            for (int j = i + 1; j < handsPressedArray.Length; j++)
            {
                var distance = Vector3.Distance(handsPressedArray[i].position, handsPressedArray[j].position);
                if (distance < result)
                {
                    result = distance;
                }
            }
        }
        return result;
    }

    private float GetMinDistanceBetweenHands(Vector3[] handsPressedArray)
    {
        var result = float.MaxValue;
        for (int i = 0; i < handsPressedArray.Length; i++)
        {
            for (int j = i + 1; j < handsPressedArray.Length; j++)
            {
                var distance = Vector3.Distance(handsPressedArray[i], handsPressedArray[j]);
                if (distance < result)
                {
                    result = distance;
                }
            }
        }
        return result;
    }
}
