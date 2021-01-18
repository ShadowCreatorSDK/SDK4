using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {

    public class InputDeviceGGT26DofPart : InputDeviceHandPart {

        public InputDeviceGGT26Dof inputDeviceGGT26Dof {
            get { return Transition<InputDeviceGGT26Dof>(inputDeviceBase); }
        }

        public InputDeviceGGT26DofPartUI inputDeviceGGT26DofPartUI {
            get { return Transition<InputDeviceGGT26DofPartUI>(inputDevicePartUIBase); }
        }


        public InputDataGGT26Dof inputDataGGT26Dof {get; set;}

        public InputDataGetGGT26Dof inputDataGetGGT26Dof { get; set; }
        public InputDeviceGGT26DofPartStatus inputDeviceGGT26DofPartStatus { get; set; }




        protected override void ModuleCreater() {
            inputDataBase = inputDataHand = inputDataGGT26Dof = new InputDataGGT26Dof(this);
            inputDataGetBase = inputDataGetHand = inputDataGetGGT26Dof = new InputDataGetGGT26Dof(this);
            inputDevicePartStatusBase = inputDevicePartStatusHand = inputDeviceGGT26DofPartStatus = new InputDeviceGGT26DofPartStatus(this);
            base.ModuleCreater();
        }
    }
}
