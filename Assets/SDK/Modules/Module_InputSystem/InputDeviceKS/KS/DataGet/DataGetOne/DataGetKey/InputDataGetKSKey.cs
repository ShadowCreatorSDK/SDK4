using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDataGetKSKey : InputDataGetGCKey {

        public InputDataGetKS inputDataGetKS;

        public InputDataGetKSKey(InputDataGetKS _inputDataGetKS) : base(_inputDataGetKS) {
            inputDataGetKS = _inputDataGetKS;
        }

        public SCModule GetKeyModule;

        public override void OnSCStart() {
            base.OnSCStart();

            if (inputDataGetKS.inputDeviceKSPart.inputDataGC.GCType == GCType.K102) {
                GetKeyModule = new GetK102Key(this);
            } else if(inputDataGetKS.inputDeviceKSPart.inputDataGC.GCType == GCType.K101) {
                GetKeyModule = new GetK101Key(this);
            } else if(inputDataGetKS.inputDeviceKSPart.inputDataGC.GCType == GCType.K11) {
                GetKeyModule = new GetK11Key(this); 
            }

            AddModule(GetKeyModule);
            GetKeyModule.ModuleStart();
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            RemoveAllModule();
            GetKeyModule = null;
        }


        public override void OnSCDestroy() {
            base.OnSCDestroy();
            GetKeyModule = null;
            inputDataGetKS = null;
        }



    }
}
