using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class TouchPointer : PointerBase, INearPointer 
    {
        public HandDetector handDetector {
            get {
                return detectorBase as HandDetector;
            }
        }

        public float TouchRadius { get => 0.2f; }
        public bool IsNearObject { get => newClosestTouchable!=null;  }
        public override PointerType PointerType => PointerType.Touch;
        private Collider[] queryBuffer=new Collider[60];

        public Action<bool> TargetDetectModelChange;


        SCPointEventData scPointEventData {
            get {
                return handDetector.inputDevicePartBase.inputDataBase.SCPointEventData;
            }
        }

        private Vector3 mPreviousTouchPosition;
        public Vector3 PreviousTouchPosition {
            get {
                if(mPreviousTouchPosition == Vector3.zero) {
                    mPreviousTouchPosition = TouchPosition;
                }
                return mPreviousTouchPosition;
            }
            private set {
                mPreviousTouchPosition = value;
            }
        }

        public Vector3 TouchPosition {
            get {
                //return gT26DofDetector.inputDeviceGT26DofPart.inputDeviceGT26DofPartUI.modelGT26Dof.TouchCursor.transform.position;
                return handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.One).transform.position;
            }
        }
        public float DistToTouchable {
            get;
            private set;
        }

        float closestDistance;
        Vector3 closestNormal;
        BaseNearInteractionTouchable newClosestTouchable;

        Vector3 start, end;
        public override void OnSCStart() {
            //base.OnSCStart();

            cursorBase?.ModuleStart();
        }
        public override void OnSCDisable() {
            base.OnSCDisable();
            if(currentPokeDownObject) {
                PokeUp(ref currentPokeDownObject, ref currentTouchableObject);
            }
            IsFocusLocked = false;
            PreviousTouchPosition = Vector3.zero;
        }

        protected override void UpdateTransform() {

            start = TouchPosition + TouchRadius * closestNormal;
            end = TouchPosition - TouchRadius * closestNormal;
            transform.rotation = Quaternion.LookRotation(end - start);
            transform.position = start;

        }
        
        private GameObject currentPokeDownObject = null;

        private BaseNearInteractionTouchable currentTouchableObject = null;

        protected override void DoTargetDetect() {
            SCInputModule.Instance.ProcessCS(scPointEventData, transform, LayerMask, (end - start).magnitude);

            IsFocusLocked = currentPokeDownObject != null;

            //Debug.Log(scPointEventData.pointerCurrentRaycast.gameObject);

            if(scPointEventData.pointerCurrentRaycast.gameObject) {

                DistToTouchable = Vector3.Distance(transform.position, scPointEventData.pointerCurrentRaycast.worldPosition) - TouchRadius;
                bool newIsDown = (DistToTouchable < 0.0f);
                bool newIsUp = (DistToTouchable > newClosestTouchable.DebounceThreshold);

                if(newIsDown && currentPokeDownObject == null) {

                    if(IsObjectPartOfTouchable(scPointEventData.pointerCurrentRaycast.gameObject, newClosestTouchable)) {
                        currentPokeDownObject = scPointEventData.pointerCurrentRaycast.gameObject;
                        currentTouchableObject = newClosestTouchable;

                        PokeDown(currentPokeDownObject, currentTouchableObject);
                    }
                } else if(currentPokeDownObject) {
                    if(newIsUp) {
                        PokeUp(ref currentPokeDownObject, ref currentTouchableObject);
                    } else {
                        PokeUpdated(currentPokeDownObject, currentTouchableObject);
                    }
                }
            } else {
                if(currentPokeDownObject) {
                    PokeUp(ref currentPokeDownObject, ref currentTouchableObject);
                }
            }

            PreviousTouchPosition = TouchPosition;

        }

        private void PokeDown(GameObject _target, BaseNearInteractionTouchable touchableObj) {
            if(touchableObj.EventsToReceive == TouchableEventType.Pointer) {
                handDetector.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Enter, InputKeyState.DOWN);
            } else if(touchableObj.EventsToReceive == TouchableEventType.Touch) {
                HandDispatcher.OnPokeDown(touchableObj.gameObject, this, scPointEventData);
            }
        }
        private void PokeUp(ref GameObject _target, ref BaseNearInteractionTouchable touchableObj) {
            if(touchableObj.EventsToReceive == TouchableEventType.Pointer) {
                handDetector.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(InputKeyCode.Enter, InputKeyState.UP);
            } else if(touchableObj.EventsToReceive == TouchableEventType.Touch) {
                HandDispatcher.OnPokeUp(touchableObj.gameObject, this, scPointEventData);
            }

            touchableObj = null;
            _target = null;
        }

        private void PokeUpdated(GameObject _target, BaseNearInteractionTouchable touchableObj) {
            if(touchableObj.EventsToReceive == TouchableEventType.Pointer) {

            } else if(touchableObj.EventsToReceive == TouchableEventType.Touch) {
                HandDispatcher.OnPokeUpdated(touchableObj.gameObject, this, scPointEventData);
            }
        }

        private static bool IsObjectPartOfTouchable(GameObject targetObject, BaseNearInteractionTouchable touchable) {
            return targetObject != null && touchable != null &&
                               (targetObject == touchable.gameObject ||
                               // Descendant game objects are touchable as well. In particular, this is needed to be able to send
                               // touch events to Unity UI control elements.
                               (targetObject.transform != null && touchable.gameObject.transform != null &&
                               targetObject.transform.IsChildOf(touchable.gameObject.transform)));
        }

        public bool FindClosestTouchableForLayerMask() {

            newClosestTouchable = null;
            closestDistance = float.PositiveInfinity;
            closestNormal = Vector3.zero;

            int numColliders = Physics.OverlapSphereNonAlloc(TouchPosition, TouchRadius, queryBuffer, LayerMask);
            if(numColliders == queryBuffer.Length) {
                Debug.LogWarning($"Maximum number of {numColliders} colliders found in PokePointer overlap query. Consider increasing the query buffer size in the input system settings.");
            }

            for(int i = 0; i < numColliders; i++) {

                var collider = queryBuffer[i];
                var touchable = collider.GetComponent<BaseNearInteractionTouchable>();
                if(touchable) {
                    float distance = touchable.DistanceToTouchable(TouchPosition, out Vector3 normal);
                    if(distance < closestDistance) {
                        newClosestTouchable = touchable;
                        closestDistance = distance;
                        closestNormal = normal;
                    }
                }
            }
            
            // Unity UI does not provide an equivalent broad-phase test to Physics.OverlapSphere,
            // so we have to use a static instances list to test all NearInteractionTouchableUnityUI
            for(int i = 0; i < NearInteractionTouchableUnityUI.Instances.Count; i++) {
                NearInteractionTouchableUnityUI touchable = NearInteractionTouchableUnityUI.Instances[i];
                float distance = touchable.DistanceToTouchable(TouchPosition, out Vector3 normal);
                if(distance <= TouchRadius && distance < closestDistance) {
                    newClosestTouchable = touchable;
                    closestDistance = distance;
                    closestNormal = normal;
                }
            }

            //if(newClosestTouchable != null) {
            //    Debug.Log("newClosestTouchable:" + newClosestTouchable.name);
            //}

            return newClosestTouchable != null;

        }


        public bool TryGetNearGraspPoint(out Vector3 position) {
            position = Vector3.zero;
            return false;
        }

        public bool TryGetNearGraspAxis(out Vector3 axis) {
            axis = transform.forward;
            return true;
        }

        public bool TryGetDistanceToNearestSurface(out float distance) {
            distance = closestDistance;
            return true;
        }

        public bool TryGetNormalToNearestSurface(out Vector3 normal) {
            normal = closestNormal;
            return true;
        }


        void OnDrawGizmos() {
            if(Application.isPlaying) {

                Gizmos.color = Color.black*0.2f;
                Gizmos.DrawSphere(transform.position, 0.01f);
                Gizmos.color = Color.black * 0.1f;
                Gizmos.DrawSphere(TouchPosition, TouchRadius);

                Gizmos.color = Color.blue * 0.3f;
                Gizmos.DrawSphere(start, 0.02f);

                Gizmos.color = Color.blue * 0.2f;
                Gizmos.DrawSphere(end, 0.01f);
            }
        }

    }
}
