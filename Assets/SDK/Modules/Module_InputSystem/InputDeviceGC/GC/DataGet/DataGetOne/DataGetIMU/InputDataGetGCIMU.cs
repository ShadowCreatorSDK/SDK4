using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public abstract class InputDataGetGCIMU : InputDataGetOneBase {

        public InputDataGetGC inputDataGetGC;
        public InputDataGetGCIMU(InputDataGetGC _inputDataGetGC) : base(_inputDataGetGC) {
            inputDataGetGC = _inputDataGetGC;
        }

        protected int[] temp = new int[3] { 0, 0, 0 }, temp1;

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            temp1 = null;
            temp = null;
        }

        public abstract int[] GetAcc();

        public abstract int[] GetGyro();

    }
}
