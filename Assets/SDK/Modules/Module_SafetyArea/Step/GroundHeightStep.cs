using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHeightStep : AbstractSafetyAreaStep
{
    private float planeHeight;

    public GroundHeightStep()
    {
        ResetPlaneHeight();
    }

    public void SetHeadPosition(Vector3 headPosition)
    {
        float largestHeight = headPosition.y - PlayAreaConstant.DEFAULT_HEIGHT_FROM_HEAD;
        if (largestHeight < planeHeight)
        {
            planeHeight = largestHeight;
        }
    }

    public void SetPlaneHeight(float interactionObjectHeight)
    {
        if (interactionObjectHeight < planeHeight)
        {
            planeHeight = interactionObjectHeight;
        }
    }

    public float GetPlaneHeight()
    {
        return planeHeight;
    }

    public void ResetPlaneHeight()
    {
        planeHeight = float.MaxValue;
    }
}