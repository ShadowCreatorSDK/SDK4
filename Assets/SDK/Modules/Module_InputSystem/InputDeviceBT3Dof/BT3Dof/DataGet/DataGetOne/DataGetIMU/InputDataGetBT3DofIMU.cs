using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDataGetBT3DofIMU : InputDataGetGCIMU {

        public InputDataGetBT3Dof inputDataGetBT3Dof;
        public InputDataGetBT3DofIMU(InputDataGetBT3Dof _inputDataGetBT3Dof) : base(_inputDataGetBT3Dof) {
            inputDataGetBT3Dof = _inputDataGetBT3Dof;
        }

        public override int[] GetAcc() {
            if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCOne) {
                temp1 = AndroidPluginBase.ObjectFunctionCall<int[]>(AndroidPluginBT3Dof.BT3DofManager, "getAcc", 0);
            } else if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCTwo) {
                temp1 = AndroidPluginBase.ObjectFunctionCall<int[]>(AndroidPluginBT3Dof.BT3DofManager, "getAcc", 1);
            }
            if(temp1 != null) {
                return temp1;
            }
            return temp;
        }

        public override int[] GetGyro() {
            if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCOne) {
                temp1 = AndroidPluginBase.ObjectFunctionCall<int[]>(AndroidPluginBT3Dof.BT3DofManager, "getGyro", 0);
            } else if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCTwo) {
                temp1 = AndroidPluginBase.ObjectFunctionCall<int[]>(AndroidPluginBT3Dof.BT3DofManager, "getGyro", 1);
            }
            if(temp1 != null) {
                return temp1;
            }
            return temp;
        }
    }
}
