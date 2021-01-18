using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class GrabPointer : PointerBase,INearPointer {
        public HandDetector handDetector {
            get {
                return detectorBase as HandDetector;
            }
        }

        public float GrabActiveRadius { get => 0.1f; }

        private float GrabRadius { get => 0.035f; }

        public bool IsNearObject { get; set; }

        public override PointerType PointerType =>PointerType.Grab;


        private Collider[] queryBuffer=new Collider[30];


        RaycastResult grabSimlateResult;

        Vector3 OverlapSphereCenter {
            get {
                return (handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.One).position +
                 handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.thumb, JOINT.One).transform.position) / 2.0f;
            }
        }

        float closestDistance;
        Vector3 closestNormal;
        NearInterationGrabbable newClosestGrabbable;
        Vector3 closestPoint;

        Vector3 start, end;

        public override void OnSCStart() {
            //base.OnSCStart();

            //cursorBase?.ModuleStart();
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            IsFocusLocked = false;
        }
        protected override void UpdateTransform() {
            if(newClosestGrabbable) {

                start = OverlapSphereCenter;
                end = closestPoint; 
                if(end - start != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(end - start);
                } else {
                    transform.rotation = Quaternion.identity;
                }
                transform.position = start;
            }
        }

        protected override void DoTargetDetect() {
            if(IsFocusLocked || newClosestGrabbable) {
                SCInputModule.Instance.ProcessCS(handDetector.inputDevicePartBase.inputDataBase.SCPointEventData, transform, LayerMask, MaxDetectDistance, true, grabSimlateResult);
                IsFocusLocked = handDetector.inputDevicePartBase.inputDataBase.SCPointEventData.DownPressGameObject != null;
            }
        }

        public bool FindClosestGrabbableForLayerMask() {

            newClosestGrabbable = null;
            closestDistance = float.PositiveInfinity;
            IsNearObject = false;

            int num = Physics.OverlapSphereNonAlloc(OverlapSphereCenter, GrabActiveRadius, queryBuffer, LayerMask);

            for(int i = 0; i < num; i++) {
                var collider = queryBuffer[i];
                var nearInteration = collider.GetComponent<NearInterationGrabbable>();

                IsNearObject = nearInteration != null;
                if(IsNearObject) {
                    break;
                }
            }

            if(IsNearObject == false) {
                return false;
            }

            num = Physics.OverlapSphereNonAlloc(OverlapSphereCenter, GrabRadius, queryBuffer, LayerMask);
            
            Vector3 objectHitPoint = Vector3.zero;

            for(int i = 0; i < num; i++) {
                var collider = queryBuffer[i];
                var touchable = collider.GetComponent<NearInterationGrabbable>();
                if(touchable) {
                    
                    objectHitPoint = collider.ClosestPoint(OverlapSphereCenter);
                    float distance = (objectHitPoint - OverlapSphereCenter).magnitude;

                    if(distance < closestDistance) {
                        newClosestGrabbable = touchable;
                        closestDistance = distance;
                        closestPoint = objectHitPoint;
                    }
                }
            }

            if(newClosestGrabbable != null) {
                //Debug.Log("newClosestGarbbable:" + closest.name);
                grabSimlateResult.gameObject = newClosestGrabbable.gameObject;
                grabSimlateResult.worldPosition = closestPoint;
            }

            return IsNearObject;

        }

        public bool TryGetNearGraspPoint(out Vector3 position) {
            throw new NotImplementedException();
        }

        public bool TryGetNearGraspAxis(out Vector3 axis) {
            throw new NotImplementedException();
        }

        public bool TryGetDistanceToNearestSurface(out float distance) {
            throw new NotImplementedException();
        }

        public bool TryGetNormalToNearestSurface(out Vector3 normal) {
            throw new NotImplementedException();
        }

        void OnDrawGizmos() {
            if(Application.isPlaying) {

                Gizmos.color = Color.black * 0.3f;
                Gizmos.DrawSphere(transform.position, 0.01f);
                Gizmos.color = Color.black * 0.2f;
                Gizmos.DrawSphere(OverlapSphereCenter, GrabRadius);
                Gizmos.color = Color.black * 0.1f;
                Gizmos.DrawSphere(OverlapSphereCenter, GrabActiveRadius);

                if(newClosestGrabbable) {
                    Gizmos.color = Color.blue * 0.3f;
                    Gizmos.DrawSphere(start, 0.02f);
                    Gizmos.color = Color.blue * 0.2f;
                    Gizmos.DrawSphere(end, 0.01f);
                }
            }
        }

    }
}
