using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class InputDataGetBT3DofTouch : InputDataGetGCTouch {
        public InputDataGetBT3Dof inputDataGetBT3Dof;
        public InputDataGetBT3DofTouch(InputDataGetBT3Dof _inputDataGetBT3Dof) : base(_inputDataGetBT3Dof) {
            inputDataGetBT3Dof = _inputDataGetBT3Dof;
        }


        private int[] pos = new int[] { 0, 0 };

        public override void UpdateTpPosition() {

            TpPosition[0] = TpPosition[1] = 0;

            if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCOne) {
                pos = AndroidPluginBase.ObjectFunctionCall<int[]>(AndroidPluginBT3Dof.BT3DofManager, "getTouchPosition", 0);
            } else if(inputDataGetBT3Dof.inputDeviceBT3DofPart.PartType == InputDevicePartType.GCTwo) {
                pos = AndroidPluginBase.ObjectFunctionCall<int[]>(AndroidPluginBT3Dof.BT3DofManager, "getTouchPosition", 1);
            } else {
                return;
            }

            if(pos == null)
                return;

            TpPosition.x = pos[0];
            TpPosition.y = pos[1];
        }
    }
}
