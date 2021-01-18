using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity
{
    public class SCTransformUtility
    {
        public static bool ScreenPointToWorldPointInPlane(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
        {
            worldPoint = Vector2.zero;
            Ray ray = cam.ScreenPointToRay(screenPoint);
            var plane = new Plane(transform.rotation * Vector3.back, transform.position);

            float dist;
            if (!plane.Raycast(ray, out dist))
                return false;

            worldPoint = ray.GetPoint(dist);
            return true;
        }

        public static bool ScreenPointToLocalPointInPlane(Transform transform, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
        {
            localPoint = Vector2.zero;
            Vector3 worldPoint;
            if (ScreenPointToWorldPointInPlane(transform, screenPoint, cam, out worldPoint))
            {
                localPoint = transform.InverseTransformPoint(worldPoint);
                return true;
            }
            return false;
        }
    }
}
