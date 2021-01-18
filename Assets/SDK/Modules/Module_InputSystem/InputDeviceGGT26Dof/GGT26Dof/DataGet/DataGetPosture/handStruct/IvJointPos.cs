using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class IvJointPos {
        private Vector3 position;
        private Quaternion rotation;

        public static IvJointPos ZeroIdentity { get; } = new IvJointPos(Vector3.zero, Quaternion.identity);

        public IvJointPos(Vector3 position, Quaternion rotation) {
            this.position = position;
            this.rotation = rotation;
        }

        public IvJointPos(Vector3 position) {
            this.position = position;
            rotation = Quaternion.identity;
        }

        public IvJointPos(Quaternion rotation) {
            position = Vector3.zero;
            this.rotation = rotation;
        }

        public Vector3 Position { get { return position; } set { position = value; } }

        public Quaternion Rotation { get { return rotation; } set { rotation = value; } }

        /// <summary>
        /// The Z axis of the pose in world space.
        /// </summary>
        public Vector3 Forward => rotation * Vector3.forward;

        /// <summary>
        /// The Y axis of the pose in world space.
        /// </summary>
        public Vector3 Up => rotation * Vector3.up;

        /// <summary>
        /// The X axis of the pose in world space.
        /// </summary>
        public Vector3 Right => rotation * Vector3.right;
    }
}
