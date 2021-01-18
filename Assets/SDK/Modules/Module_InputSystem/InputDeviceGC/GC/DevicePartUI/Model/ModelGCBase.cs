using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public abstract class ModelGCBase : ModelBase {
        public InputDeviceGCPartUI inputDeviceGCPartUI {
            get {
                return inputDevicePartUIBase as InputDeviceGCPartUI;
            }
        }

        public Transform StartPoint;

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            SetTpPosition();
            SetHandleKeysColor();
        }


        public virtual void SetTpPosition() {

        }
        

        public virtual void SetHandleKeysColor() {
            
        }


    }
}