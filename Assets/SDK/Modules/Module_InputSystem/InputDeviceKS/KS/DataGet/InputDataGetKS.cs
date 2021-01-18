using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDataGetKS : InputDataGetGC {

        public InputDeviceKSPart inputDeviceKSPart;

        public InputDataGetKSPosture inputDataGetKSPosture;
        public InputDataGetKSKey inputDataGetKSKey;
        public InputDataGetKSJoystick inputDataGetKSJoystick;
        public InputDataGetKSIMU inputDataGetKSIMU;
        public InputDataGetKSHall inputDataGetKSHall;
        public InputDataGetKS(InputDeviceKSPart inputDeviceKSPart) :base(inputDeviceKSPart) {
            this.inputDeviceKSPart = inputDeviceKSPart;

            dataGetOneList.Add(inputDataGetGCPosture = inputDataGetKSPosture = new InputDataGetKSPosture(this));
            dataGetOneList.Add(inputDataGetGCKey = inputDataGetKSKey = new InputDataGetKSKey(this));
            dataGetOneList.Add(inputDataGetKSJoystick = new InputDataGetKSJoystick(this));
            dataGetOneList.Add(inputDataGetKSHall = new InputDataGetKSHall(this));
        }
    }
}
