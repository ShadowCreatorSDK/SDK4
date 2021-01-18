using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// Use a Unity UI RectTransform as touchable surface.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class NearInteractionTouchableUnityUI : BaseNearInteractionTouchable {
        private Lazy<RectTransform> rectTransform;

        public static IReadOnlyList<NearInteractionTouchableUnityUI> Instances => instances;


        private static readonly List<NearInteractionTouchableUnityUI> instances = new List<NearInteractionTouchableUnityUI>();

        public NearInteractionTouchableUnityUI() {
            rectTransform = new Lazy<RectTransform>(GetComponent<RectTransform>);
        }


        public override float DistanceToTouchable(Vector3 samplePoint, out Vector3 normal) {

            normal = Normal;

            Vector3 localPoint = transform.InverseTransformPoint(samplePoint);

            // touchables currently can only be touched within the bounds of the rectangle.
            // We return infinity to ensure that any point outside the bounds does not get touched.
            if(!rectTransform.Value.rect.Contains(localPoint)) {
                return float.PositiveInfinity;
            }

            // Scale back to 3D space
            localPoint = TransformSize(transform,localPoint);

            return Math.Abs(localPoint.z);
        }

        /// <summary>
        /// Transforms the size from local to world.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="localSize">The local size.</param>
        /// <returns>World size.</returns>
        public Vector3 TransformSize(Transform transform, Vector3 localSize) {
            Vector3 transformedSize = new Vector3(localSize.x, localSize.y, localSize.z);

            Transform t = transform;
            do {
                transformedSize.x *= t.localScale.x;
                transformedSize.y *= t.localScale.y;
                transformedSize.z *= t.localScale.z;
                t = t.parent;
            }
            while(t != null);

            return transformedSize;
        }

        protected void OnEnable() {
            instances.Add(this);
        }

        protected void OnDisable() {
            instances.Remove(this);
        }
    }
}
