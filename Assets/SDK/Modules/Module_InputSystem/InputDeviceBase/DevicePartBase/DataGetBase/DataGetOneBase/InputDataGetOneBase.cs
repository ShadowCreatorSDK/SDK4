
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class InputDataGetOneBase : SCModule {

        public InputDataGetBase inputDataGetBase;
        public InputDataGetOneBase(InputDataGetBase _inputDataGetBase) {
            inputDataGetBase = _inputDataGetBase;
        }

    }
}
