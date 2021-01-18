using System.Collections;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDeviceKSPartUI : InputDeviceGCPartUI {
        public InputDeviceKSPart inputDeviceKSPart {
            get {
                return inputDevicePartBase as InputDeviceKSPart;
            }
        }

        private ModelK102 modelK102 { get { return GetComponentInChildren<ModelK102>(true); } }
        private ModelK101 modelK101 { get { return GetComponentInChildren<ModelK101>(true); } }
        private ModelK11 modelK11 { get { return GetComponentInChildren<ModelK11>(true); } }

        public override void OnSCAwake() {
            base.OnSCAwake();
            RemoveModule(modelBase);
            AddModule(modelK102);
            AddModule(modelK101);
            AddModule(modelK11);
        }

        public override void OnSCStart() {
            base.OnSCStart();
            modelBase.ModuleStop();
            if (inputDeviceKSPart.inputDataGC.GCType == GCType.K102 && modelK102) {
                modelBase = ModelGC = modelK102;
            }else if (inputDeviceKSPart.inputDataGC.GCType == GCType.K101 && modelK101) {
                modelBase = ModelGC =  modelK101;
            } else if(inputDeviceKSPart.inputDataGC.GCType == GCType.K11 && modelK11) {
                modelBase = ModelGC = modelK11;
            }

            AddModule(modelBase);
            modelBase?.ModuleStart();
            DebugMy.Log("Model Type:" + modelBase?.GetType(),this,true);
        }


        public override void OnSCDisable() {
            base.OnSCDisable();
            RemoveAllModule();
            modelBase = ModelGC = null;
        }

    }
}