using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {

    public class InputDeviceBT3DofPart : InputDeviceGCPart {

        public InputDeviceBT3Dof inputDeviceBT3Dof {
            get { return Transition<InputDeviceBT3Dof>(inputDeviceBase); }
        }

        public InputDeviceBT3DofPartUI inputDeviceBT3DofPartUI {
            get { return Transition<InputDeviceBT3DofPartUI>(inputDevicePartUIBase); }
        }
        public BT3DofDetector bT3DofDetector {
            get { return Transition<BT3DofDetector>(detectorBase); }
        }

        public override PostureType PostureType {
            get {
                return PostureType._3Dof;
            }
            set => throw new NotImplementedException();
        }
        public InputDataBT3Dof inputDataBT3Dof {get; set;}

        public InputDataGetBT3Dof inputDataGetBT3Dof { get; set; }


        public InputDeviceBT3DofPartStatus inputDevicePartStatusBT3Dof { get; set; }


        public InputDevicePartDispatchEventBT3Dof inputDevicePartDispatchEventBT3Dof { get; set; }


        protected override void ModuleCreater() {
            inputDataBase = inputDataGC = inputDataBT3Dof = new InputDataBT3Dof(this);
            inputDataGetBase = inputDataGetGC = inputDataGetBT3Dof = new InputDataGetBT3Dof(this);
            inputDevicePartStatusBase = inputDevicePartStatusGC = inputDevicePartStatusBT3Dof = new InputDeviceBT3DofPartStatus(this);
            inputDevicePartDispatchEventBase = inputDevicePartDispatchEventGC = inputDevicePartDispatchEventBT3Dof = new InputDevicePartDispatchEventBT3Dof(this);
        }

    }
}
