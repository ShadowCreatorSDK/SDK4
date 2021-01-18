using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDeviceBT3Dof : InputDeviceGC {

        public override InputDeviceType inputDeviceType {
            get {
                return InputDeviceType.BT3Dof;
            }
        }


        public BT3DofConnectListener bT3DofConnectListener;


        [Header("Enable GameController")]
        public bool OneGCActive = true;
        public bool TwoGCActive = false;
        protected override void InputDeviceStart() {
            SetActiveInputDevicePart(InputDevicePartType.GCOne, OneGCActive);
            SetActiveInputDevicePart(InputDevicePartType.GCTwo, TwoGCActive);
        }

        public override void OnSCAwake() {
            base.OnSCAwake();
            if(Application.platform == RuntimePlatform.Android) {
                bT3DofConnectListener = new BT3DofConnectListener(this);
                AndroidPluginBase.ObjectAddListener(AndroidPluginBT3Dof.BT3DofManager, "setHandShankConnStateCallback", bT3DofConnectListener);
            }
        }


        public override void OnSCDestroy() {
            base.OnSCDestroy();
            bT3DofConnectListener = null;
            if(Application.platform == RuntimePlatform.Android) {
                AndroidPluginBase.ObjectAddListener(AndroidPluginBT3Dof.BT3DofManager, "setHandShankConnStateCallback", null);
            }
        }
    }
}
