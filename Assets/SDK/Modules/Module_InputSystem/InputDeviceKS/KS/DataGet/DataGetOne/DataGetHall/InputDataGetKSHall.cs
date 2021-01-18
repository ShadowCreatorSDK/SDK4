using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDataGetKSHall : InputDataGetOneBase {
        public InputDataGetKS inputDataGetKS;
        public InputDataGetKSHall(InputDataGetKS _inputDataGetKS) : base(_inputDataGetKS) {
            inputDataGetKS = _inputDataGetKS;
        }


        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateHall();
        }
        public virtual void UpdateHall() {
            if(InputDataKS.TempHallDataList.Count > 0) {

                if((inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSLeft && InputDataKS.TempHallDataList[0].deviceID == 0)
                    ||
                   (inputDataGetKS.inputDeviceKSPart.PartType == InputDevicePartType.KSRight && InputDataKS.TempHallDataList[0].deviceID == 1)) {

                    inputDataGetKS.inputDeviceKSPart.inputDataKS.HallFoward = InputDataKS.TempHallDataList[0].HallFoward;
                    inputDataGetKS.inputDeviceKSPart.inputDataKS.HallInside = InputDataKS.TempHallDataList[0].HallInside;

                    InputDataKS.TempHallDataList.RemoveAt(0);

                    DebugMy.Log(inputDataGetKS.inputDeviceKSPart.PartType + " UpdateHall:" + inputDataGetKS.inputDeviceKSPart.inputDataKS.HallFoward + " " + inputDataGetKS.inputDeviceKSPart.inputDataKS.HallInside, this, true);

                }

            }

        }


    }
}
