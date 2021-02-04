using System;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// °ÍÈû¶ûÇúÏß
    /// </summary>
    public class BezierDataProvider {

        public BezierDataProvider(LineBase lineBase) {
            this.lineBase = lineBase;
            p1Position = GetPoint(1);
            p2Position = GetPoint(2);
        }


        [Serializable]
        private struct BezierPoint {
            public BezierPoint(float spread) {
                Point1 = Vector3.right * spread * 0.5f;
                Point2 = Vector3.right * spread * 0.25f;
                Point3 = Vector3.left * spread * 0.25f;
                Point4 = Vector3.left * spread * 0.5f;
            }

            public Vector3 Point1;
            public Vector3 Point2;
            public Vector3 Point3;
            public Vector3 Point4;
        }


        /// <inheritdoc />
        public int PointCount { get { return 4; } }

        [Header("Bezier Settings")]
        [SerializeField]
        private BezierPoint controlPoints = new BezierPoint(0.5f);

        /// <summary>
        /// Returns world position of first point along line as defined by this data provider
        /// </summary>
        public Vector3 FirstPoint {
            get => GetPoint(0);
            set => SetPoint(0, value);
        }

        /// <summary>
        /// Returns world position of last point along line as defined by this data provider
        /// </summary>
        public Vector3 LastPoint {
            get => GetPoint(PointCount - 1);
            set => SetPoint(PointCount - 1, value);
        }

        /// <summary>
        /// Pointer local to world transform
        /// </summary>
        public LineBase lineBase;

        [SerializeField]
        private float inertia = 4f;
        [SerializeField]
        private float dampen = 5f;
        [SerializeField]
        private float seekTargetStrength = 50f;

        [SerializeField]
        private Vector3 p1Target = new Vector3(0, 0, 0.25f);
        [SerializeField]
        private Vector3 p2Target = new Vector3(0, 0, 0.75f);

        private Vector3 p1Velocity;
        private Vector3 p1Position;
        private Vector3 p1Offset;

        private Vector3 p2Velocity;
        private Vector3 p2Position;
        private Vector3 p2Offset;

        //[Tooltip("Where to place the first control point of the bezier curve")]
        //[SerializeField]
        //[Range(0f, 0.5f)]
        private float startPointLerp = 0.66f;

        //[SerializeField]
        //[Tooltip("Where to place the second control point of the bezier curve")]
        //[Range(0.5f, 1f)]
        private float endPointLerp = 0.88f;

        public void UpdatePointe1_2() {

            Vector3 p1BasePoint = GetPoint(1);
            Vector3 p2BasePoint = GetPoint(2);

            p1Offset = p1BasePoint - p1Position;
            p2Offset = p2BasePoint - p2Position;

            Vector3 temp1 = p1Target;
            Vector3 temp2 = p2Target;

            if ((GetPoint(3) - GetPoint(0)).magnitude > 1) {
                p1Target = Vector3.forward * 0.5f;
                p2Target = Vector3.forward * 0.8f;
            } else {
                p1Target = Vector3.forward * (GetPoint(3) - GetPoint(0)).magnitude / 4;
                p2Target = Vector3.forward * (GetPoint(3) - GetPoint(0)).magnitude / 2;
            }

            p1Target = Vector3.Lerp(temp1, p1Target, 0.5f);
            p2Target = Vector3.Lerp(temp2, p2Target, 0.5f);

            Vector3 p1WorldTarget = lineBase.transform.TransformPoint(p1Target);
            Vector3 p2WorldTarget = lineBase.transform.TransformPoint(p2Target);

            p1Offset += p1WorldTarget - p1Position;
            p2Offset += p2WorldTarget - p2Position;

            p1Velocity = Vector3.Lerp(p1Velocity, p1Offset, Time.deltaTime * inertia);
            p1Velocity = Vector3.Lerp(p1Velocity, Vector3.zero, Time.deltaTime * dampen);

            p2Velocity = Vector3.Lerp(p2Velocity, p2Offset, Time.deltaTime * inertia);
            p2Velocity = Vector3.Lerp(p2Velocity, Vector3.zero, Time.deltaTime * dampen);

            p1Position = p1Position + p1Velocity;
            p2Position = p2Position + p2Velocity;

            p1Position = Vector3.Lerp(p1Position, p1WorldTarget, seekTargetStrength * Time.deltaTime);
            p2Position = Vector3.Lerp(p2Position, p2WorldTarget, seekTargetStrength * Time.deltaTime);

            SetPoint(1, p1Position);
            SetPoint(2, p2Position);


        }


        /// <summary>
        /// Gets a point along the line at the specified index
        /// </summary>
        public Vector3 GetPoint(int pointIndex) {
            if(pointIndex < 0 || pointIndex >= PointCount) {
                Debug.LogError("Invalid point index");
                return Vector3.zero;
            }

            Vector3 point = GetPointInternal(pointIndex);
            return lineBase.transform.TransformPoint(point);
        }
        
        /// <summary>
        /// Gets a point along the line at the specified normalized length.
        /// </summary>
        public Vector3 GetPoint(float normalizedLength) {
            //normalizedLength = Mathf.Lerp(lineStartClamp, lineEndClamp, Mathf.Clamp01(normalizedLength));
            Vector3 point = GetPointInternal(controlPoints.Point1, controlPoints.Point2, controlPoints.Point3, controlPoints.Point4, normalizedLength);
            point = lineBase.transform.TransformPoint(point);
            //DistortPoint(ref point, normalizedLength);
            return point;
        }

        /// <inheritdoc />
        protected Vector3 GetPointInternal(int pointIndex) {
            switch(pointIndex) {
                case 0:
                    return controlPoints.Point1;

                case 1:
                    return controlPoints.Point2;

                case 2:
                    return controlPoints.Point3;

                case 3:
                    return controlPoints.Point4;

                default:
                    return Vector3.zero;
            }
        }

        /// <summary>
        /// Sets a point in the line
        /// This function is not guaranteed to have an effect
        /// </summary>
        public void SetPoint(int pointIndex, Vector3 point) {
            if(pointIndex < 0 || pointIndex >= PointCount) {
                Debug.LogError("Invalid point index");
                return;
            }

            point = lineBase.transform.InverseTransformPoint(point);
            SetPointInternal(pointIndex, point);
        }

        protected void SetPointInternal(int pointIndex, Vector3 point) {
            switch(pointIndex) {
                case 0:
                    controlPoints.Point1 = point;
                    break;

                case 1:
                    controlPoints.Point2 = point;
                    break;

                case 2:
                    controlPoints.Point3 = point;
                    break;

                case 3:
                    controlPoints.Point4 = point;
                    break;

                default:
                    break;
            }
        }

        /// <inheritdoc />
        protected Vector3 GetPointInternal(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, float normalizedLength) {
            float invertedDistance = 1f - normalizedLength;
            return invertedDistance * invertedDistance * invertedDistance * point1 +
                   3f * invertedDistance * invertedDistance * normalizedLength * point2 +
                   3f * invertedDistance * normalizedLength * normalizedLength * point3 +
                   normalizedLength * normalizedLength * normalizedLength * point4;
        }

        /// <summary>
        /// Gets the normalized distance along the line path (range 0 to 1) going the given number of steps provided
        /// </summary>
        /// <param name="stepNum">Number of steps to take "walking" along the curve </param>
        protected virtual float GetNormalizedPointAlongLine(int linePointCount,int stepNum) {
            return (1f / (linePointCount - 1)) * stepNum;
        }

        Vector3[] positions;
        public Vector3[] GetLinePointers(int pointerCount) {
            if (positions == null || positions.Length != pointerCount) {
                positions = new Vector3[pointerCount];
            }
            for(int i = 0; i < positions.Length; i++) {
                float normalizedDistance = GetNormalizedPointAlongLine(pointerCount,i);
                positions[i] = GetPoint(normalizedDistance);
            }
            return positions;
        }
    }
}