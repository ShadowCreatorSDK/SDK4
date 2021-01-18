using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public abstract class InputDataGetGCTouch : InputDataGetOneBase {
        public InputDataGetGC inputDataGetGC;
        public InputDataGetGCTouch(InputDataGetGC _inputDataGetGC) : base(_inputDataGetGC) {
            inputDataGetGC = _inputDataGetGC;
        }


        protected Vector2 TpPosition = new Vector2();

        public override void OnSCLateUpdate() {

            UpdateTpPosition();

            inputDataGetGC.inputDeviceGCPart.inputDataGC.InputDataAddTouch(TpPosition);
        }

        public abstract void UpdateTpPosition();
    }
}
