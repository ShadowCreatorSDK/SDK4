using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {
    public class InputDeviceHead : InputDeviceBase {

        public override InputDeviceType inputDeviceType {
            get {
                return InputDeviceType.Head;
            }
        }
    }
}
