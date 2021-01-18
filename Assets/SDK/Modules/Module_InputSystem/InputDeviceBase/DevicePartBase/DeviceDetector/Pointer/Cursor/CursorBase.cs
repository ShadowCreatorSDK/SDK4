using UnityEngine;
using System;
using System.Collections.Generic;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class CursorBase : SCModuleMono {

        PointerBase mPointerBase;
        public PointerBase pointerBase {
            get {
                if(mPointerBase == null) {
                    mPointerBase = GetComponentInParent<PointerBase>();
                }
                return mPointerBase;
            }
            private set {
                mPointerBase = value;
            }
        }

        Gazeloading mGazeloading;
        public Gazeloading Gazeloading
        {
            get
            {
                if (mGazeloading == null)
                {
                    mGazeloading = GetComponentInChildren<Gazeloading>(true);
                }
                return mGazeloading;
            }
            private set
            {
                mGazeloading = value;
            }
        }

        public override void OnSCAwake()
        {
            base.OnSCAwake();
            AddModule(Gazeloading);
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateTransform();
            UpdateCursorVisual();
        }
        public override void OnSCDestroy() {
            base.OnSCDestroy();
            pointerBase = null;
        }

        public virtual void UpdateTransform() { }
        
        public abstract void UpdateCursorVisual();

        public void StartGazeAnimation(float timer) {
            Gazeloading.timer = timer;
            Gazeloading.ModuleStart();
        }
        public void StopGazeAnimation()
        {
            Gazeloading.ModuleStop();

        }
    }
}
