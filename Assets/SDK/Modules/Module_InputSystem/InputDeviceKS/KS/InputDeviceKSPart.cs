using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {

    public class InputDeviceKSPart : InputDeviceGCPart {

        public InputDeviceKS inputDeviceKS {
            get { return Transition<InputDeviceKS>(inputDeviceBase); }
        }

        public InputDeviceKSPartUI inputDeviceKSPartUI {
            get { return Transition<InputDeviceKSPartUI>(inputDevicePartUIBase); }
        }
        public KSDetector bT3DofDetector {
            get { return Transition<KSDetector>(detectorBase); }
        }

        private PostureType mPostureType = PostureType.UnKown;
        public override PostureType PostureType {
            get {
                return mPostureType;
            }
            set {
                mPostureType = value;
            }
        }

        public InputDataKS inputDataKS {get; set;}

        public InputDataGetKS inputDataGetKS { get; set; }


        public InputDeviceKSPartStatus inputDevicePartStatusKS { get; set; }


        public InputDevicePartDispatchEventKS inputDevicePartDispatchEventKS { get; set; }


        protected override void ModuleCreater() {
            inputDataBase = inputDataGC = inputDataKS = new InputDataKS(this);
            inputDataGetBase = inputDataGetGC = inputDataGetKS = new InputDataGetKS(this);
            inputDevicePartStatusBase = inputDevicePartStatusGC = inputDevicePartStatusKS = new InputDeviceKSPartStatus(this);
            inputDevicePartDispatchEventBase = inputDevicePartDispatchEventGC = inputDevicePartDispatchEventKS = new InputDevicePartDispatchEventKS(this);
        }

    }
}
