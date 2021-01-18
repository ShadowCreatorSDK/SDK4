using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class InputDataGetHand : InputDataGetBase {

        public InputDeviceHandPart inputDeviceHandPart;


        public InputDataGetHandPosture inputDataGetHandPosture;
        public InputDataGetHandsData inputDataGetHandsData;
        public InputDataGetKey inputDataGetKey;
        public InputDataGetHand(InputDeviceHandPart _inputDeviceHandPart) : base(_inputDeviceHandPart) {
            inputDeviceHandPart = _inputDeviceHandPart;

            dataGetOneList.Add(inputDataGetKey = new InputDataGetHandKey(this));
        }
    }
}
