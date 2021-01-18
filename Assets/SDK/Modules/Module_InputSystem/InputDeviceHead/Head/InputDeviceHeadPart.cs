using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {

    public class InputDeviceHeadPart : InputDevicePartBase {

        
        public InputDataHead inputDataHead {
            get {
                return Transition<InputDataHead>(inputDataBase);
            }
        }

        public InputDataGetHead inputDataGetHead {
            get {
                return Transition<InputDataGetHead>(inputDataBase);
            }
        }


        public InputDeviceHeadPartStatus inputDevicePartStatusHead {
            get {
                return inputDevicePartStatusBase as InputDeviceHeadPartStatus;
            }
        }


        public InputDevicePartDispatchEventHead inputDevicePartDispatchEventHead {
            get {
                return inputDevicePartDispatchEventBase as InputDevicePartDispatchEventHead;
            }
        }

        protected override void ModuleCreater() {
            inputDataBase = new InputDataHead(this);
            inputDataGetBase = new InputDataGetHead(this);
            inputDevicePartStatusBase = new InputDeviceHeadPartStatus(this);
            inputDevicePartDispatchEventBase = new InputDevicePartDispatchEventHead(this);
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
