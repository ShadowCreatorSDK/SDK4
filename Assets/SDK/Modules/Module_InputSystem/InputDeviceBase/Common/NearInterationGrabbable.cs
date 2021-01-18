using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public class NearInterationGrabbable : MonoBehaviour {

        public Transform GrabbaleTransform;
        BoxCollider boxCollider;


        public virtual float DistanceToGrabbable(Vector3 samplePoint) {

            if(GrabbaleTransform == null) {
                GrabbaleTransform = transform;
            }


            if(boxCollider == null) {
                boxCollider = GrabbaleTransform.GetComponent<BoxCollider>();
                if(boxCollider == null) {
                    return float.PositiveInfinity;
                }
            }

            return (samplePoint - GrabbaleTransform.position).magnitude;
        }

    }
}
