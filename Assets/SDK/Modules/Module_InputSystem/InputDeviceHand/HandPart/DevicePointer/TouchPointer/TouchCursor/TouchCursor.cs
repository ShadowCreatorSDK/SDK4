using UnityEngine;
using DG.Tweening;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class TouchCursor : DefaultCursor {

        public TouchPointer touchPointer {
            get {
                return Transition<TouchPointer>(pointerBase);
            }
        }
        
        [SerializeField]
        [Tooltip("Positional offset from the finger's skin surface.")]
        private float skinSurfaceOffset = 0.01f;

        [Header("Ring Visualization")]
        [SerializeField]
        [Tooltip("Renderer representing the ring attached to the index finger using an MRTK/Standard material with the round corner feature enabled.")]
        protected Renderer indexFingerRingRenderer;
       
        /// <summary>
        /// When lerping, use unscaled time. This is useful for games that have a pause mechanism or otherwise adjust the game timescale.
        /// </summary>
        public bool UseUnscaledTime {
            get { return useUnscaledTime; }
            set { useUnscaledTime = value; }
        }

        [Header("Motion")]
        [SerializeField]
        [Tooltip("When lerping, use unscaled time. This is useful for games that have a pause mechanism or otherwise adjust the game timescale.")]
        private bool useUnscaledTime = true;

        /// <summary>
        /// Blend value for surface normal to user facing lerp.
        /// </summary>
        public float PositionLerpTime {
            get { return positionLerpTime; }
            set { positionLerpTime = value; }
        }

        [SerializeField]
        [Tooltip("Blend value for surface normal to user facing lerp")]
        private float positionLerpTime = 0.01f;
        /// <summary>
        /// Blend value for surface normal to user facing lerp.
        /// </summary>
        public float RotationLerpTime {
            get { return rotationLerpTime; }
            set { rotationLerpTime = value; }
        }

        [SerializeField]
        [Tooltip("Blend value for surface normal to user facing lerp")]
        private float rotationLerpTime = 0.01f;
        [SerializeField]
        [Tooltip("At what distance should the cursor align with the surface. (Should be < alignWithFingerDistance)")]
        private float alignWithSurfaceDistance = 0.1f;
        
        private MaterialPropertyBlock materialPropertyBlock;
        private int proximityDistanceID;
        private Transform forefingerOne {
            get {
                return touchPointer ? touchPointer.handDetector.inputDeviceHandPart.inputDeviceHandPartUI
                    .modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.One).transform : null;
            }
        }
        private Transform forefingerFour {
            get {
                return touchPointer ? touchPointer.handDetector.inputDeviceHandPart.inputDeviceHandPartUI
                    .modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).transform : null;
            }
        }

        public override void OnSCAwake() {
            base.OnSCAwake();
            materialPropertyBlock = new MaterialPropertyBlock();
            proximityDistanceID = Shader.PropertyToID("_Proximity_Distance_");
        }

        public override void UpdateCursorVisual() {
           // throw new System.NotImplementedException();
        }

        public override void UpdateTransform() {
            if(touchPointer == null) {
                DebugMy.Log("touchPointer == null", this, true);
                return;
            }
            float deltaTime = UseUnscaledTime? Time.unscaledDeltaTime: Time.deltaTime;

            Vector3 indexFingerPosition = forefingerOne.position;
            Quaternion indexFingerRotation = forefingerOne.rotation;
            Vector3 indexKnucklePosition = forefingerFour.position;

            if(touchPointer.IsNearObject) {
                float distance;
                if(!touchPointer.TryGetDistanceToNearestSurface(out distance)) {
                    distance = float.MaxValue;
                }
                if(indexFingerRingRenderer != null) {
                    TranslateToFinger(indexFingerRingRenderer.transform, deltaTime, indexFingerPosition, indexKnucklePosition);

                    Vector3 surfaceNormal;
                    if((distance < alignWithSurfaceDistance) &&
                        touchPointer.TryGetNormalToNearestSurface(out surfaceNormal)) {
                        RotateToSurfaceNormal(indexFingerRingRenderer.transform, surfaceNormal, indexFingerRotation, distance);
                        TranslateFromTipToPad(indexFingerRingRenderer.transform, indexFingerPosition, indexKnucklePosition, surfaceNormal, distance);
                    } else {
                        RotateToFinger(indexFingerRingRenderer.transform, deltaTime, indexFingerRotation);
                    }

                    UpdateVisuals(indexFingerRingRenderer, distance, true);
                }
            } else {
                // If the pointer is disabled, make sure to turn the ring cursor off
                // but still want show the proximity effect on bounding content
                if(indexFingerRingRenderer != null) {
                    UpdateVisuals(indexFingerRingRenderer, 1, false);
                }
            }
        }
        /// <summary>
        /// Applies material overrides to a ring renderer.
        /// </summary>
        /// <param name="ringRenderer">Renderer using an MRTK/Standard material with the round corner feature enabled.</param>
        /// <param name="distance">Distance between the ring and surface.</param>
        /// <param name="visible">Should the ring be visible?</param>
        protected virtual void UpdateVisuals(Renderer ringRenderer, float distance, bool visible) {
            ringRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetFloat(proximityDistanceID, visible ? distance : 1.0f);
            ringRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        private void TranslateToFinger(Transform target, float deltaTime, Vector3 fingerPosition, Vector3 knucklePosition) {
            var targetPosition = fingerPosition + (fingerPosition - knucklePosition).normalized * skinSurfaceOffset;
            target.position = Vector3.Lerp(target.position, targetPosition, deltaTime / PositionLerpTime);
        }
        private void RotateToFinger(Transform target, float deltaTime, Quaternion pointerRotation) {
            target.rotation = Quaternion.Lerp(target.rotation, pointerRotation, deltaTime / RotationLerpTime);
        }
        private void RotateToSurfaceNormal(Transform target, Vector3 surfaceNormal, Quaternion pointerRotation, float distance) {
            var t = distance / alignWithSurfaceDistance;
            var targetRotation = Quaternion.LookRotation(-surfaceNormal);
            target.rotation = Quaternion.Slerp(targetRotation, pointerRotation, t);
        }
        private void TranslateFromTipToPad(Transform target, Vector3 fingerPosition, Vector3 knucklePosition, Vector3 surfaceNormal, float distance) {
            var t = distance / alignWithSurfaceDistance;

            Vector3 tipNormal = (fingerPosition - knucklePosition).normalized;
            Vector3 tipPosition = fingerPosition + tipNormal * skinSurfaceOffset;
            Vector3 tipOffset = tipPosition - fingerPosition;

            // Check how perpendicular the finger normal is to the surface, so that the cursor will
            // not translate to the finger pad if the user is poking with a horizontal finger
            float fingerSurfaceDot = Vector3.Dot(tipNormal, -surfaceNormal);

            // Lerping an angular measurement from 0 degrees (default cursor position at tip of finger) to
            // 90 degrees (a new position on the fingertip pad) around the fingertip's X axis.
            Quaternion degreesRelative = Quaternion.AngleAxis((1f - t) * 90f * (1f - fingerSurfaceDot), indexFingerRingRenderer.transform.right);

            Vector3 tipToPadPosition = fingerPosition + degreesRelative * tipOffset;
            indexFingerRingRenderer.transform.position = tipToPadPosition;
        }
    }
}
