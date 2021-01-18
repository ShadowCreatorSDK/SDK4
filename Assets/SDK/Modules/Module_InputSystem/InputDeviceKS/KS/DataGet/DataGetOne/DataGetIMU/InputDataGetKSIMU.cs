using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDataGetKSIMU : InputDataGetGCIMU {

        public InputDataGetKS inputDataGetKS;
        public InputDataGetKSIMU(InputDataGetKS _inputDataGetKS) : base(_inputDataGetKS) {
            inputDataGetKS = _inputDataGetKS;
        }

        public override int[] GetAcc() {

            return temp;
        }

        public override int[] GetGyro() {

            return temp;
        }
    }
}
