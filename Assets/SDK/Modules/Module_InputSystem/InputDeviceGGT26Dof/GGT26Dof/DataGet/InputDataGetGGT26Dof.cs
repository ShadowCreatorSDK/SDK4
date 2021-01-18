using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class InputDataGetGGT26Dof : InputDataGetHand {

        public InputDeviceGGT26DofPart inputDeviceGGT26DofPart;


        public InputDataGetGGT26DofPosture inputDataGetGGT26DofPosture;
        public InputDataGGT26DofGetHandsData inputDataGGT26DofGetHandsData;

        public InputDataGetGGT26Dof(InputDeviceGGT26DofPart _inputDeviceGGT26DofPart) : base(_inputDeviceGGT26DofPart) {
            inputDeviceGGT26DofPart = _inputDeviceGGT26DofPart;

            dataGetOneList.Add( inputDataGetHandsData = inputDataGGT26DofGetHandsData = new InputDataGGT26DofGetHandsData(this));
            dataGetOneList.Add(inputDataGetGGT26DofPosture = new InputDataGetGGT26DofPosture(this));
        }
    }
}
