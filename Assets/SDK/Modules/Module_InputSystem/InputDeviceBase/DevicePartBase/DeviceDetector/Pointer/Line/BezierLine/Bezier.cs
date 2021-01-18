using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// 巴塞尔曲线
    /// </summary>
    public class Bezier {

        Transform transform; 
        private float inertia = 10f;
        private float dampen = 1f;
        private float seekTargetStrength = 1f;

        private Vector3 p1Target = new Vector3(0, 0, 0.05f);
        private Vector3 p2Target = new Vector3(0, 0, 0.15f);

        private Vector3 p1Velocity;
        private Vector3 p1Position;
        private Vector3 p1Offset;

        private Vector3 p2Velocity;
        private Vector3 p2Position;
        private Vector3 p2Offset;

        /// <summary>
        /// 四个Point,一条直线上，是相对于某个Transform而言的局部坐标
        /// </summary>
        public Vector3[] controlPoints = new Vector3[4] {
            Vector3.forward * 0f,
            Vector3.forward * 0f,
            Vector3.forward * 0f,
            Vector3.forward
        };

        public Bezier(Transform _transform) {
            transform = _transform;
            p1Position = GetPoint(1);
            p2Position = GetPoint(2);
        }

        /// <summary>
        /// 设置controlPoints中的某个点的位置，参数Point是全局坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="pointIndex"></param>
        /// <param name="point"></param>
        public void SetPoint(int pointIndex, Vector3 point) {
            if(pointIndex < 0 || pointIndex >= controlPoints.Length) {
                //Debug.LogError("Invalid point index");
                return;
            }
            controlPoints[pointIndex] = transform.InverseTransformPoint(point);
        }

        /// <summary>
        /// 获取controlPoints中的某个点的位置，是全局坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="pointIndex"></param>
        /// <returns></returns>
        public Vector3 GetPoint(int pointIndex) {
            if(pointIndex < 0 || pointIndex >= controlPoints.Length) {
                //Debug.LogError("Invalid point index");
                return Vector3.zero;
            }
            return transform.TransformPoint(controlPoints[pointIndex]);
        }

        public Vector3 GetPoint(float normalizedLength) {
            return transform.TransformPoint(GetPointInternal(normalizedLength));
        }

        protected Vector3 GetPointInternal(float normalizedDistance) {
            return InterpolateBezierPoints(controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], normalizedDistance);
        }




        public Vector3 InterpolateBezierPoints(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, float normalizedLength) {
            float invertedDistance = 1f - normalizedLength;
            return invertedDistance * invertedDistance * invertedDistance * point1 +
                   3f * invertedDistance * invertedDistance * normalizedLength * point2 +
                   3f * invertedDistance * normalizedLength * normalizedLength * point3 +
                   normalizedLength * normalizedLength * normalizedLength * point4;
        }
        protected virtual float GetNormalizedPointAlongLine(int stepNum) {
            return (1f / (24 - 1)) * stepNum;
        }
        public Vector3[] UpdateControlPointsAndGetLinePoints(int linePointNum) {

            Vector3 p1BasePoint = GetPoint(1);
            Vector3 p2BasePoint = GetPoint(2);
            Vector3 p1TargetTemp, p2TargetTemp;

            p1Offset = p1BasePoint - p1Position;
            p2Offset = p2BasePoint - p2Position;

            if((GetPoint(3) - GetPoint(0)).magnitude > 1) {
                p1TargetTemp = Vector3.forward * 0.5f;
                p2TargetTemp = Vector3.forward * 0.8f;
            } else {
                p1TargetTemp = Vector3.forward * (GetPoint(3) - GetPoint(0)).magnitude / 4;
                p2TargetTemp = Vector3.forward * (GetPoint(3) - GetPoint(0)).magnitude / 2;
            }

            p1Target = Vector3.Lerp(p1Target, p1TargetTemp, 0.5f);
            p2Target = Vector3.Lerp(p2Target, p2TargetTemp, 0.5f);

            Vector3 p1WorldTarget = transform.TransformPoint(p1Target);
            Vector3 p2WorldTarget = transform.TransformPoint(p2Target);

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


            Vector3[] positions = new Vector3[linePointNum];
            for(int i = 0; i < positions.Length; i++) {
                float normalizedDistance = GetNormalizedPointAlongLine(i);
                positions[i] = GetPoint(normalizedDistance);
            }
            return positions;
        }
    }
}