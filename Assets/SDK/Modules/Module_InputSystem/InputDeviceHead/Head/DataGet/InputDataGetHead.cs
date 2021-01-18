using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {
    public class InputDataGetHead:InputDataGetBase {

        public InputDeviceHeadPart inputDeviceHeadPart;
        public InputDataGetHead(InputDeviceHeadPart _inputDeviceHeadPart) :base(_inputDeviceHeadPart) {
            inputDeviceHeadPart = _inputDeviceHeadPart;


            dataGetOneList.Add(new InputDataGetKey(this));
            dataGetOneList.Add(new InputDataGetPosture(this));
        }

    }
}
