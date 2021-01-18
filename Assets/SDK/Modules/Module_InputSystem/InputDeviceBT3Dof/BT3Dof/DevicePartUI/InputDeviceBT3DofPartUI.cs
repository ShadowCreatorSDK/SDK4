using System.Collections;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDeviceBT3DofPartUI : InputDeviceGCPartUI {
        public InputDeviceBT3DofPart inputDeviceBT3DofPart {
            get {
                return inputDevicePartBase as InputDeviceBT3DofPart;
            }
        }

        private ModelK02 modelK02 { get { return GetComponentInChildren<ModelK02>(true); } }
        private ModelK07 modelK07 { get { return GetComponentInChildren<ModelK07>(true); } }


        public override void OnSCAwake() {
            base.OnSCAwake();
            RemoveModule(modelBase);
            AddModule(modelK02);
            AddModule(modelK07);
        }

        public override void OnSCStart() {
            base.OnSCStart();
            modelBase.ModuleStop();
            if(inputDeviceBT3DofPart.inputDataBT3Dof.GCType == GCType.K07 && modelK07) {
                modelBase = ModelGC = modelK07;
            } else if(inputDeviceBT3DofPart.inputDataBT3Dof.GCType == GCType.K02 && modelK02) {
                modelBase = ModelGC = modelK02;
            }

            modelBase?.ModuleStart();
            DebugMy.Log("Model Type:" + modelBase?.GetType()+" "+ modelK02.IsModuleStarted + " " + modelK07.IsModuleStarted, this, true);
        }


        public override void OnSCDisable() {
            base.OnSCDisable();
            RemoveAllModule();
            modelBase = ModelGC = null;
        }

    }
}