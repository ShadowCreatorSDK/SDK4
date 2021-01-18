using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SC.XR.Unity.Module_InputSystem {

    public class SCInputModule : StandaloneInputModule {

        public static SCInputModule Instance { get; private set; }

        private Camera mRayCasterCamera;
        public Camera RayCasterCamera {
            get {
                if(mRayCasterCamera == null) {
                    mRayCasterCamera = EnsureUIEventCamera();
                }
                return mRayCasterCamera;
            }
        }

        private PhysicsRaycaster mPhysicsRaycaster;
        public PhysicsRaycaster PhysicsRaycaster {
            get {
                if(mPhysicsRaycaster == null) {
                    mPhysicsRaycaster = EnsurePhysicsRaycaster();
                }
                return mPhysicsRaycaster;
            }
        }

        public LayerMask layerMask; 
        public float maxDetectDistance=0;

        Vector3? lastMousePoint3d;
        Vector2 newPos, lastPosition;
        Vector3 viewportPos = new Vector3(0.5f, 0.5f, 1.0f);

        protected override void Start() {
            base.Start();
            if(Instance) {
                DestroyImmediate(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public override void Process() {

        }

        public virtual void ProcessCS(SCPointEventData scPointEventData, Transform posture, int layerMask = 1 << 8, float maxDetectDistance = 30,bool isSimlate=false, RaycastResult simlateResult=new RaycastResult()) {

            //if(!eventSystem.isFocused)
            //    return;

            bool usedEvent = SendUpdateEventToSelectedObject();

            ProcessSCEventInit(scPointEventData, posture, layerMask, maxDetectDistance);
            ProcessSCEvent(scPointEventData, isSimlate, simlateResult);

            if(eventSystem.sendNavigationEvents) {
                if(!usedEvent)
                    usedEvent |= SendMoveEventToSelectedObject();

                if(!usedEvent)
                    SendSubmitEventToSelectedObject();
            }

        }

        void ProcessSCEventInit(SCPointEventData scPointEventData, Transform posture, int layerMask, float maxDetectDistance) {
            if(this.layerMask.value != 0) {
                layerMask = this.layerMask;
            }
            if(this.maxDetectDistance > 0) {
                maxDetectDistance = this.maxDetectDistance;
            }

            PhysicsRaycaster.eventMask = layerMask;

            RayCasterCamera.cullingMask = layerMask;
            RayCasterCamera.farClipPlane = maxDetectDistance;

            RayCasterCamera.transform.position = posture.position; // posture.position;// Vector3.Lerp(RayCasterCamera.transform.position, targetDetectBase.transform.position,0.3f); //targetDetectBase.transform.position;
            RayCasterCamera.transform.rotation = posture.rotation;

            ///Canvas分配wrold Camera
            foreach(var canvas in CanvasCollection.CanvasList) {
                canvas.worldCamera = RayCasterCamera;
            }

            newPos = SCInputModule.Instance.RayCasterCamera.ViewportToScreenPoint(viewportPos);

            // Populate initial data or drag data
            if(lastMousePoint3d == null) {
                // For the first event, use the same position for 'last' and 'new'.
                lastPosition = newPos;
            } else {
                // Otherwise, re-project the last pointer position.
                lastPosition = SCInputModule.Instance.RayCasterCamera.WorldToScreenPoint(lastMousePoint3d.Value);
            }

            // Save off the 3D position of the cursor.
            lastMousePoint3d = SCInputModule.Instance.RayCasterCamera.ViewportToWorldPoint(viewportPos);

            scPointEventData.delta = newPos - lastPosition;
            scPointEventData.position = newPos;

            if(RayCasterCamera != null && Application.platform != RuntimePlatform.Android) {
                Ray ray = new Ray();
                ray = RayCasterCamera.ScreenPointToRay(scPointEventData.position);
                Debug.DrawRay(ray.origin, ray.direction * maxDetectDistance, Color.blue);
            }
        }

        /// <summary>
        /// Process all mouse events.
        /// </summary>
        protected void ProcessSCEvent(SCPointEventData scPointEventData, bool isSimlate = false, RaycastResult simlateResult = new RaycastResult()) {

            scPointEventData.MouseButtonEventData.buttonData = scPointEventData;
            scPointEventData.MouseButtonEventData.buttonState = StateForMouseButton(scPointEventData.inputDevicePartBase);

            if(isSimlate) {
                scPointEventData.pointerCurrentRaycast = simlateResult;
            } else {
                eventSystem.RaycastAll(scPointEventData, m_RaycastResultCache);
                scPointEventData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
                m_RaycastResultCache.Clear();
            }

            


            scPointEventData.Forward = RayCasterCamera.transform.forward;

            if(scPointEventData.MouseButtonEventData.PressedThisFrame() && scPointEventData.pointerCurrentRaycast.gameObject) {

                scPointEventData.HitPointerRelativeRayCasterCamera = RayCasterCamera.transform.InverseTransformPoint(scPointEventData.pointerCurrentRaycast.worldPosition);
                scPointEventData.Position3D = scPointEventData.PressPosition3D = RayCasterCamera.transform.TransformPoint(scPointEventData.HitPointerRelativeRayCasterCamera);
                scPointEventData.pressForward = RayCasterCamera.transform.forward;
                scPointEventData.DownPressGameObject = scPointEventData.pointerCurrentRaycast.gameObject;

            } else if(scPointEventData.MouseButtonEventData.ReleasedThisFrame()) {

                scPointEventData.HitPointerDeltaDragObjCenter = scPointEventData.HitPointerRelativeRayCasterCamera = scPointEventData.dragObjPosition = Vector3.zero;
                scPointEventData.Position3D = Vector3.zero;
                scPointEventData.PressPosition3D = Vector3.zero;
                scPointEventData.DownPressGameObject = null;
                scPointEventData.pressForward = Vector3.zero;

            }


            if(scPointEventData.DownPressGameObject) {
                scPointEventData.Position3D = RayCasterCamera.transform.TransformPoint(scPointEventData.HitPointerRelativeRayCasterCamera);
            }

            // Process the first mouse button fully
            ProcessMousePress(scPointEventData.MouseButtonEventData);
            ProcessMove(scPointEventData);


            if(scPointEventData.MouseButtonEventData.PressedThisFrame() && scPointEventData.pointerCurrentRaycast.gameObject && scPointEventData.pointerDrag) {
                ///碰撞点指向碰撞物体中心的向量
                scPointEventData.HitPointerDeltaDragObjCenter = scPointEventData.pointerDrag.transform.position - scPointEventData.pointerCurrentRaycast.worldPosition;
                ///第一次赋值 ，must
                scPointEventData.dragObjPosition = scPointEventData.HitPointerDeltaDragObjCenter + scPointEventData.Position3D;
            }

            ///必须在ProcessDrag前，否则，延迟一帧导致停顿感
            if(scPointEventData.dragging) {
                scPointEventData.dragObjPosition = scPointEventData.HitPointerDeltaDragObjCenter + scPointEventData.Position3D;
            }

            ///拖拽不启动阈值，否则不能触发IDragHandler
            //scPointEventData.useDragThreshold = false;

            ProcessDrag(scPointEventData);

            ///更新dragHitPinterPosition，必须在ProcessDrag后，ProcessDrag调用的DragHandler可能会更改PointerDrag对象的Transform
            if(scPointEventData.dragging) {
                scPointEventData.dragAnchorPosition3D = scPointEventData.pointerDrag.transform.position - scPointEventData.HitPointerDeltaDragObjCenter;
            } else {
                scPointEventData.dragAnchorPosition3D = Vector3.zero;
            }


            if(!Mathf.Approximately(scPointEventData.scrollDelta.sqrMagnitude, 0.0f)) {
                var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(scPointEventData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.ExecuteHierarchy(scrollHandler, scPointEventData, ExecuteEvents.scrollHandler);
            }

            scPointEventDataxx = scPointEventData;

        }

        private static bool ShouldStartDrag(Vector3 pressPos, Vector3 currentPos, float threshold, bool useDragThreshold) {
            if(!useDragThreshold)
                return true;

            return (pressPos - currentPos).magnitude >= threshold * 0.0005f;
        }

        protected override void ProcessDrag(PointerEventData pointerEvent) {
            if(!pointerEvent.IsPointerMoving() ||
                Cursor.lockState == CursorLockMode.Locked ||
                pointerEvent.pointerDrag == null)
                return;

            if(!pointerEvent.dragging
                && ShouldStartDrag((pointerEvent as SCPointEventData).PressPosition3D, (pointerEvent as SCPointEventData).Position3D, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold)) {
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);

                pointerEvent.dragging = true;
            }

            // Drag notification
            if(pointerEvent.dragging) {

                // Before doing drag we should cancel any pointer down state
                // And clear selection!
                if(pointerEvent.pointerPress != pointerEvent.pointerDrag) {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                    pointerEvent.eligibleForClick = false;
                    pointerEvent.pointerPress = null;
                    pointerEvent.rawPointerPress = null;
                }
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            }

        }


        protected PointerEventData.FramePressState StateForMouseButton(InputDevicePartBase devicePart) {
            var pressed = devicePart.inputDataBase.inputKeys.GetKeyDown(InputKeyCode.Enter);
            var released = devicePart.inputDataBase.inputKeys.GetKeyUp(InputKeyCode.Enter);

            if(pressed && released)
                return PointerEventData.FramePressState.PressedAndReleased;
            if(pressed)
                return PointerEventData.FramePressState.Pressed;
            if(released)
                return PointerEventData.FramePressState.Released;
            return PointerEventData.FramePressState.NotChanged;
        }

        protected PhysicsRaycaster EnsurePhysicsRaycaster() {

            mPhysicsRaycaster = GetComponent<PhysicsRaycaster>();
            if(mPhysicsRaycaster == null) {
                mPhysicsRaycaster = gameObject.AddComponent<PhysicsRaycaster>();
            }
            return mPhysicsRaycaster;
        }

        protected Camera EnsureUIEventCamera(float maxDetectDistance = 30) {
            mRayCasterCamera = GetComponent<Camera>();
            if(!mRayCasterCamera) {
                mRayCasterCamera = gameObject.AddComponent<Camera>();
            }

            mRayCasterCamera.enabled = false;
            mRayCasterCamera.nearClipPlane = 0;
            mRayCasterCamera.farClipPlane = maxDetectDistance;
            mRayCasterCamera.clearFlags = CameraClearFlags.Color;
            mRayCasterCamera.backgroundColor = Color.black;
            mRayCasterCamera.orthographic = true;
            mRayCasterCamera.allowHDR = false;
            mRayCasterCamera.allowMSAA = false;
            mRayCasterCamera.orthographicSize = 0.1f;
            mRayCasterCamera.aspect = 1f;
            return mRayCasterCamera;
        }



        SCPointEventData scPointEventDataxx;
        //void OnDrawGizmos() {
        //    if(Application.isPlaying) {

        //        // Now show the input position.
        //        //Gizmos.color = Color.red;
        //        //foreach(var item in cornerLocalPostion) {
        //        //    Gizmos.DrawSphere(item, 0.01f);
        //        //}
        //        //Gizmos.color = Color.yellow;
        //        //foreach(var item in midPointLocalPostion) {
        //        //    Gizmos.DrawSphere(item, 0.01f);
        //        //}
        //        if(scPointEventDataxx != null && scPointEventDataxx.DownPressGameObject) {
        //            Gizmos.color = Color.black;
        //            Gizmos.DrawSphere(scPointEventDataxx.PressPosition3D, 0.01f);
        //            //Gizmos.color = Color.red * 0.5f;
        //            Gizmos.DrawSphere(scPointEventDataxx.Position3D, 0.01f);
        //            //Gizmos.color = Color.yellow * 0.5f;
        //            if(scPointEventDataxx.pointerDrag != null) {
        //                Gizmos.DrawSphere(scPointEventDataxx.dragObjPosition, 0.04f);
        //                //Gizmos.color = Color.blue * 0.8f;
        //                Gizmos.DrawSphere(scPointEventDataxx.dragAnchorPosition3D, 0.02f);
        //            }

        //        }
        //        //if(scPointEventDataxx.pointerDrag)
        //        //Gizmos.DrawSphere(scPointEventDataxx.pointerDrag.transform.position, 0.01f);
        //    }
        //}


    }
}
