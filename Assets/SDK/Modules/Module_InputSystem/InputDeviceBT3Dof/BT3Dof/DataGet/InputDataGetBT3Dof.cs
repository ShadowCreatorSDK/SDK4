using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDataGetBT3Dof : InputDataGetGC {

        public InputDeviceBT3DofPart inputDeviceBT3DofPart;

        public InputDataGetBT3DofPosture inputDataGetBT3DofPosture;
        public InputDataGetBT3DofKey inputDataGetBT3DofKey;
        public InputDataGetBT3DofTouch inputDataGetBT3DofTouch;
        public InputDataGetBT3DofIMU inputDataGetBT3DofIMU;
        public InputDataGetBT3Dof(InputDeviceBT3DofPart inputDeviceBT3DofPart) :base(inputDeviceBT3DofPart) {
            this.inputDeviceBT3DofPart = inputDeviceBT3DofPart;

            dataGetOneList.Add(inputDataGetGCPosture = inputDataGetBT3DofPosture = new InputDataGetBT3DofPosture(this));
            dataGetOneList.Add(inputDataGetGCKey = inputDataGetBT3DofKey = new InputDataGetBT3DofKey(this));
            dataGetOneList.Add(inputDataGetGCTouch = inputDataGetBT3DofTouch = new InputDataGetBT3DofTouch(this));
            dataGetOneList.Add(inputDataGetGCIMU = inputDataGetBT3DofIMU = new InputDataGetBT3DofIMU(this));
        }
    }
}
