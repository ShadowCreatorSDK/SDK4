using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// 负责射线检测
    /// </summary>
    public abstract class PointerBase : SCModuleMono ,IPointer{


        DetectorBase mDetectorBase;
        public DetectorBase detectorBase {
            get {
                if(mDetectorBase == null) {
                    mDetectorBase = GetComponentInParent<DetectorBase>();
                }
                return mDetectorBase;
            }
            private set {
                mDetectorBase = value;
            }
        }


        CursorBase _CursorBase;
        public CursorBase cursorBase {
            get {
                if(_CursorBase == null) {
                    _CursorBase = GetComponentInChildren<CursorBase>(true);
                }
                return _CursorBase;
            }
            set {
                _CursorBase = value;
            }
        }

        LineBase _LineBase;
        public LineBase lineBase {
            get {
                if(_LineBase == null) {
                    _LineBase = GetComponentInChildren<LineBase>(true);
                }
                return _LineBase;
            }
            set {
                _LineBase = value;
            }
        }


        public float MaxDetectDistance = 30.0f;
        public LayerMask LayerMask;
        public bool IsFocusLocked { get; set; }

        public RayStep Ray { get; }
        public abstract PointerType PointerType { get;}

        public SCPointEventData ResultData => detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData;


        #region Module Behavior
        public override void OnSCAwake() {
            base.OnSCAwake();
            if(LayerMask.value == 0) {
                SetLayerMask(~(1 << 8));
            }

            AddModule(cursorBase);
            AddModule(lineBase);
        }

        public override void OnSCStart() {
            base.OnSCStart();
            
            EnsureEventSystem();

            cursorBase?.ModuleStart();
            lineBase?.ModuleStart();
        }

        public override void OnSCLateUpdate() {
            UpdateTransform();
            DoTargetDetect();
            base.OnSCLateUpdate();


        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            IsFocusLocked = false;
            //SCInputModule.Instance.ProcessCS(detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData, transform, LayerMask, MaxDetectDistance);
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            detectorBase = null;
            cursorBase = null;
            lineBase = null;
        }

        #endregion

        protected virtual void UpdateTransform() {
            transform.position = detectorBase.inputDevicePartBase.inputDataBase.position;
            transform.rotation = detectorBase.inputDevicePartBase.inputDataBase.rotation;
        }

        protected virtual void DoTargetDetect() {

            ///目标检测及发送UGUI EVENts
            SCInputModule.Instance.ProcessCS(detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData, transform, LayerMask, MaxDetectDistance);
        }

        protected void EnsureEventSystem() {
            bool isHaveInputModule = false;
            if(EventSystem.current == null) {
                GameObject EventSystemObj = new GameObject("EventSystem");
                EventSystemObj.AddComponent<EventSystem>();
            }
            BaseInputModule[] inputMs = EventSystem.current.gameObject.GetComponents<BaseInputModule>();
            foreach(var item in inputMs) {
                if(item as SCInputModule) {
                    isHaveInputModule = true;
                } else {
                    DestroyImmediate(item);
                }
            }
            if(isHaveInputModule == false) {
                EventSystem.current.gameObject.AddComponent<SCInputModule>();
            }
        }

        public void SetLayerMask(params string[] layerName) {
            LayerMask = LayerMask.GetMask(layerName);
        }

        public void SetLayerMask(int layer) {
            LayerMask = layer;
        }


    }
}
