using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {

    public class GGT26DofHandInfo:handInfo {


        public GGT26DofHandInfo(string configPath) :base(configPath) {
        }

        private Vector3 LeftGreyCameraOffset = new Vector3(0.065f,0.01f,0.02f);

        public override Vector3 positionOffest {
            get {
                return LeftGreyCameraOffset + deltaOffset;
            }
            set {
                deltaOffset = value;
            }
        }
    }


}







