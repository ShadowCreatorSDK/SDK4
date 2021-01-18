using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public abstract class InputDeviceGCPart : InputDevicePartBase {

        public InputDeviceGC inputDeviceGC {
            get { return Transition<InputDeviceGC>(inputDeviceBase); }
        }

        public InputDeviceGCPartUI inputDeviceGCPartUI {
            get { return Transition<InputDeviceGCPartUI>(inputDevicePartUIBase); }
        }
        public GCDetector gcDetector {
            get { return Transition<GCDetector>(detectorBase); }
        }

        public abstract PostureType PostureType { get; set; }

        public InputDataGC inputDataGC {get; set;}

        public InputDataGetGC inputDataGetGC { get; set; }


        public InputDeviceGCPartStatus inputDevicePartStatusGC { get; set; }


        public InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC { get; set; }
        

        public override void OnSCStart() {

            if(inputDataBase != null)
                inputDataBase.ModuleStart();

            if(inputDevicePartStatusBase != null)
                inputDevicePartStatusBase.ModuleStart();

        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(inputDataBase.isVaild == false) {

                if(inputDataGetBase != null && inputDataGetBase.IsModuleStarted)
                    inputDataGetBase.ModuleStop();
            } else {

                if(inputDataGetBase != null && !inputDataGetBase.IsModuleStarted)
                    inputDataGetBase.ModuleStart();
            }

        }

    }
}
