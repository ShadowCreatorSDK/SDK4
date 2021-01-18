using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class ModelK102 : ModelK101 {
        public virtual void UpdateHallVisual() {
            if (HallForward) {
                HallForward.localPosition = (HallForwardPressLocalPosition - HallForwardReleaseLocalPosition) / (HallForwardPressValue - HallForwardReleaseValue) * inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.HallFoward + HallForwardPressLocalPosition;
                if (inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.HallFoward > (HallForwardPressValue - HallForwardReleaseValue) / 2) {
                    hallFoward.material = pressMaterial;
                } else {
                    hallFoward.material = releaseMaterial;
                }
            }
            if (HallInside) {
                HallInside.localPosition = (HallInsidePressLocalPosition - HallInsideReleaseLocalPosition) / (HallForwardPressValue - HallForwardReleaseValue) * inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.HallInside + HallInsidePressLocalPosition;
                if (inputDeviceKSPartUI.inputDeviceKSPart.inputDataKS.HallInside > (HallForwardPressValue - HallForwardReleaseValue) / 2) {
                    hallInside.material = pressMaterial;
                } else {
                    hallInside.material = releaseMaterial;
                }
            }
        }
    }
}
