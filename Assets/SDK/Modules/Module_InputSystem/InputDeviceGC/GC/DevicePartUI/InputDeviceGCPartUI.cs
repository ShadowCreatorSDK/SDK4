using System.Collections;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public class InputDeviceGCPartUI : InputDevicePartUIBase {
        public InputDeviceGCPart inputDeviceGCPart {
            get {
                return inputDevicePartBase as InputDeviceGCPart;
            }
        }

        public ModelGCBase ModelGC;

    public ResetLoading resetLoading { get { return GetComponentInChildren<ResetLoading>(true); } }

        public override void OnSCAwake() {
            base.OnSCAwake(); 
            ModelGC = modelBase as ModelGCBase;
            AddModule(resetLoading);
        }


        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            OnUpdateResetLoading();
        }


        protected virtual void OnUpdateResetLoading() {
            if(inputDeviceGCPart.inputDataGetGC.inputDataGetGCPosture == null)
                return;

            if (inputDeviceGCPart.inputDataGetGC.inputDataGetGCPosture.postureType == PostureType._3Dof) {
                if (inputDeviceGCPart.inputDataGC.inputKeys.GetKeyDown(inputDeviceGCPart.inputDataBase.CalibrationKeyAlias)) {
                    resetLoading.ModuleStart();
                } else if (inputDeviceGCPart.inputDataGC.inputKeys.GetKeyUp(inputDeviceGCPart.inputDataBase.CalibrationKeyAlias)) {
                    resetLoading.ModuleStop();
                }
            }
        }



    }
}