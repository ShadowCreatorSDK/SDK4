using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {


    public class InputDataBT3Dof : InputDataGC {

        public InputDeviceBT3DofPart inputDeviceBT3DofPart;
        public InputDataBT3Dof(InputDeviceBT3DofPart inputDeviceBT3DofPart) : base(inputDeviceBT3DofPart) {
            this.inputDeviceBT3DofPart = inputDeviceBT3DofPart;
        }

        public BT3DofIndex index {
            get {
                if (inputDeviceGCPart.PartType == InputDevicePartType.GCOne) {
                    return BT3DofIndex.BT3DofOne;
                } else if (inputDeviceGCPart.PartType == InputDevicePartType.GCTwo) {
                    return BT3DofIndex.BT3DofTwo;
                }
                return BT3DofIndex.UnKnow;
            }
        }


        public class StatusData {
            public int deviceID;
            public bool isConnected;
        }
        public static List<StatusData> StatusDataList = new List<StatusData>();
    }
}
