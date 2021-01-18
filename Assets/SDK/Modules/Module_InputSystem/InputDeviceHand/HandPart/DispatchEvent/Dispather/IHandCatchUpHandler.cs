using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public interface IHandCatchUpHandler : IEventSystemHandler {
        void OnHandCatchUp(InputDeviceHandPart inputDeviceHandPart, SCPointEventData sCPointEventData);
    }
}
