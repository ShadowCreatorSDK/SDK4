using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryAreaStep : AbstractSafetyAreaStep
{
    private Vector2 circleCenter = new Vector2(0, 0);

    public void SetCircleCenter(Vector3 headPosition)
    {
        circleCenter = new Vector2(headPosition.x, headPosition.z);
    }

    public Vector2 GetCircleCenter()
    {
        return circleCenter;
    }
}
