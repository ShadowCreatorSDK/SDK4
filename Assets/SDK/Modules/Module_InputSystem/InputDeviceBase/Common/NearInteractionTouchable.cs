using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem
{
    public class NearInteractionTouchable : BaseNearInteractionTouchable
    {
        //[SerializeField]
        BoxCollider boxCollider;
        public BoxCollider BoxCollider
        {
            get
            {
                if (boxCollider == null)
                {
                    boxCollider = gameObject.GetComponent<BoxCollider>();
                    if (boxCollider == null)
                    {
                        boxCollider = gameObject.AddComponent<BoxCollider>();
                    }
                }
                return boxCollider;
            }
        }


        public override float DistanceToTouchable(Vector3 samplePoint, out Vector3 normal)
        {
            normal = Normal;

            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider>();
                if (boxCollider == null)
                {
                    return float.PositiveInfinity;
                }
            }

            Vector3 worldForwardPlaneCenter;
            Vector3 localsamplePoint;
            float distance = 0;

            if(NormalType == NormalType.NZ) {
                worldForwardPlaneCenter = transform.position + transform.TransformVector(boxCollider.center + Vector3.forward * -boxCollider.size.z / 2f);
            } else if(NormalType == NormalType.Z) {
                worldForwardPlaneCenter = transform.position + transform.TransformVector(boxCollider.center + Vector3.forward * boxCollider.size.z / 2f);
            } else if(NormalType == NormalType.NY) {
                worldForwardPlaneCenter = transform.position + transform.TransformVector(boxCollider.center + Vector3.up * -boxCollider.size.y / 2f);
            } else if(NormalType == NormalType.Y) {
                worldForwardPlaneCenter = transform.position + transform.TransformVector(boxCollider.center + Vector3.up * boxCollider.size.y / 2f);
            } else if(NormalType == NormalType.NX) {
                worldForwardPlaneCenter = transform.position + transform.TransformVector(boxCollider.center + Vector3.right * -boxCollider.size.x / 2f);
            } else{
                worldForwardPlaneCenter = transform.position + transform.TransformVector(boxCollider.center + Vector3.right * boxCollider.size.x / 2f);
            }

            localsamplePoint = samplePoint - worldForwardPlaneCenter;

            distance = Vector3.Dot(localsamplePoint, Normal);

            // Get surface coordinates
            Vector3 planeSpacePoint = new Vector3(
                Vector3.Dot(localsamplePoint, transform.right),
                Vector3.Dot(localsamplePoint, transform.up),
                Vector3.Dot(localsamplePoint, transform.forward));

            if(NormalType == NormalType.NZ || NormalType == NormalType.Z) {
                if(planeSpacePoint.x < -(boxCollider.size.x * transform.lossyScale.x) / 2 ||
                    planeSpacePoint.x > (boxCollider.size.x * transform.lossyScale.x) / 2 ||
                    planeSpacePoint.y < -(boxCollider.size.y * transform.lossyScale.y) / 2 ||
                    planeSpacePoint.y > (boxCollider.size.y * transform.lossyScale.y) / 2) {
                    return float.PositiveInfinity;
                }
            } else if(NormalType == NormalType.NY || NormalType == NormalType.Y) {
                if(planeSpacePoint.z < -(boxCollider.size.z * transform.lossyScale.z) / 2 ||
                     planeSpacePoint.z > (boxCollider.size.z * transform.lossyScale.z) / 2 ||
                     planeSpacePoint.x < -(boxCollider.size.x * transform.lossyScale.x) / 2 ||
                     planeSpacePoint.x > (boxCollider.size.x * transform.lossyScale.x) / 2) {
                    return float.PositiveInfinity;
                }
            } else{
                if(planeSpacePoint.z < -(boxCollider.size.z * transform.lossyScale.z) / 2 ||
                     planeSpacePoint.z > (boxCollider.size.z * transform.lossyScale.z) / 2 ||
                     planeSpacePoint.y < -(boxCollider.size.y * transform.lossyScale.y) / 2 ||
                     planeSpacePoint.y > (boxCollider.size.y * transform.lossyScale.y) / 2) {
                    return float.PositiveInfinity;
                }
            }

            if (planeSpacePoint.x < -(boxCollider.size.x * transform.lossyScale.x) / 2 ||
                 planeSpacePoint.x > (boxCollider.size.x * transform.lossyScale.x) / 2 ||
                 planeSpacePoint.y < -(boxCollider.size.y * transform.lossyScale.y) / 2 ||
                 planeSpacePoint.y > (boxCollider.size.y * transform.lossyScale.y) / 2)
            {
                return float.PositiveInfinity;
            }
            return Math.Abs(distance);
        }
    }
}
