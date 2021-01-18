using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// Type of Events to receive from a PokePointer.
    /// </summary>
    public enum TouchableEventType {
        Touch,
        Pointer,
    }

    public enum NormalType { 
        X,
        Y,
        Z,
        NX,
        NY,
        NZ,
    }


    /// <summary>
    /// Base class for all NearInteractionTouchables.
    /// </summary>
    /// <remarks>

    public abstract class BaseNearInteractionTouchable : MonoBehaviour {
        [SerializeField]
        protected TouchableEventType eventsToReceive = TouchableEventType.Touch;

        /// <summary>
        /// The type of event to receive.
        /// </summary>
        public TouchableEventType EventsToReceive { get => eventsToReceive; set => eventsToReceive = value; }


        public NormalType NormalType = NormalType.NZ;

        public Vector3 Normal {
            get {
                if(NormalType == NormalType.NZ) {
                    return -transform.forward;
                } else if(NormalType == NormalType.Z) {
                    return transform.forward;
                } else if(NormalType == NormalType.X) {
                    return transform.right;
                } else if(NormalType == NormalType.NX) {
                    return -transform.right;
                } else if(NormalType == NormalType.Y) {
                    return transform.up;
                } else {
                    return -transform.up;
                }
            }
        }

        [SerializeField]
        private Transform center;

        public Transform Center {
            get {
                return center != null ? center : transform;
            }
        }


        [Tooltip("Distance in front of the surface at which you will receive a touch completed event")]
        [SerializeField]
        protected float debounceThreshold = 0.01f;
        /// <summary>
        /// Distance in front of the surface at which you will receive a touch completed event.
        /// </summary>
        /// <remarks>
        /// When the touchable is active and the pointer distance becomes greater than +DebounceThreshold (i.e. in front of the surface),
        /// then the Touch Completed event is raised and the touchable object is released by the pointer.
        /// </remarks>
        public float DebounceThreshold { get => debounceThreshold; set => debounceThreshold = value; }

        protected virtual void OnValidate() {
            debounceThreshold = Math.Max(debounceThreshold, 0);
        }

        public abstract float DistanceToTouchable(Vector3 samplePoint, out Vector3 normal);
    }
}

