using UnityEngine;
using DG.Tweening;

namespace SC.XR.Unity.Module_InputSystem {
    public class RayStep {
        public Vector3 Origin { get; private set; }
        public Vector3 Terminus { get; private set; }
        public Vector3 Direction { get; private set; }

        /// <summary>
        /// Update current raystep with new origin and terminus points. 
        /// Pass by ref to avoid unnecessary struct copy into function since values will be copied anyways locally
        /// </summary>
        /// <param name="origin">beginning of raystep origin</param>
        /// <param name="terminus">end of raystep</param>
        public void UpdateRayStep(ref Vector3 origin, ref Vector3 terminus) {
            Origin = origin;
            Terminus = terminus;

            Direction = (Terminus - Origin).normalized;
        }

    }


}
